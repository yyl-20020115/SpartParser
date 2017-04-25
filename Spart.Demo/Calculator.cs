namespace Spart.Demo
{
	using Spart.Parsers.NonTerminal;
	using Spart.Parsers;
	using Spart.Scanners;
	using Spart.Actions;
	using System.Collections.Generic;
	using Spart.Parsers.Primitives;

	/// <summary>
	/// group       ::= '(' expression ')'
	/// factor      ::= integer | group
	/// term        ::= factor (('*' factor) | ('/' factor))*
	/// expression  ::= term (('+' term) | ('-' term))*
	/// </summary>
	public class Calculator
	{
		public class Experssion
		{
			public virtual string Text { get; protected set; } = string.Empty;
			public virtual string Operation { get; protected set; } = string.Empty;
			protected List<Experssion> expressions = null;
			public virtual List<Experssion> Expressions => this.expressions ?? (this.expressions = new List<Experssion>());

			public Experssion(string Operation = "", string Text = "", params Experssion[] Expressions)
			{
				this.Operation = Operation;
				this.Text = Text;

				if (Expressions != null)
				{
					this.Expressions.AddRange(Expressions);
				}
			}
		}

		protected Rule group = new Rule(nameof(group));
		protected Rule term = new Rule(nameof(term));
		protected Rule factor = new Rule(nameof(factor));
		protected Rule expression = new Rule(nameof(expression));
		protected Rule integer = new Rule(nameof(integer));

		protected Stack<(Experssion E, double N)> Stack = new Stack<(Experssion, double)>();

		/// <summary>
		/// A very simple calculator parser
		/// </summary>
		public Calculator()
		{
			this.integer.Parser = (+Prims.Digit)[
				(parser, args) =>
				{
					string text = args?.Value;

					this.Stack.Push(
						(
							new Experssion(Text: text)
							,
							double.TryParse(text, out double N) ? N : double.NaN
						)
					);
				}
			];

			this.group.Parser
				= ((Parser)'(')[(parser, args) =>
				{

					string op = (parser as CharParser).Name;

					if (op == "(")
					{
						this.Stack.Push((new Experssion(Operation: op), double.NaN));
					}
				}
			]
				+ this.expression
				+ ((Parser)')')[(parser, args) =>
				{

					string op = (parser as CharParser).Name;

					if (op == ")" && this.Stack.Count >= 2)
					{
						var Center = this.Stack.Pop();
						var Left = this.Stack.Peek();

						if (Left.E.Operation == "(")
						{
							this.Stack.Pop();
						}

						this.Stack.Push(Center);
					}
				}
			];

			this.factor.Parser = this.group | this.integer;

			void DoMath(Parser parser, ActionEventArgs args)
			{
				string op = (parser as BinaryTerminalParser)?.FirstParser?.Name;

				if (!string.IsNullOrEmpty(op) && this.Stack.Count >= 2)
				{
					var Y = this.Stack.Pop();
					var X = this.Stack.Pop();

					double x = X.N;
					double y = Y.N;
					double z = double.NaN;

					switch (op)
					{
						case "+":
							z = x + y;
							break;
						case "-":
							z = x - y;
							break;
						case "*":
							z = x * y;
							break;
						case "/":
							z = x / y;
							break;
					}

					this.Stack.Push((new Experssion(op, string.Empty, X.E, Y.E), z));
				}
			}

			this.term.Parser = this.factor + ~(
				('*' + factor)[DoMath]
				|
				('/' + factor)[DoMath]
			);

			this.expression.Parser = this.term + ~(
				('+' + term)[DoMath]
				|
				('-' + term)[DoMath]
			);
		}

		/// <summary>
		/// Parse a string and return parse match
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		protected virtual ParserMatch InternalParse(string text)
		{
			return this.expression.Parse(new StringScanner(text));
		}
		public virtual (Experssion E, double N) Calculate(string text)
		{
			(Experssion E, double N) result = (null, double.NaN);

			if (!string.IsNullOrEmpty(text))
			{
				ParserMatch m = this.InternalParse(text);

				if (m.Success)
				{
					if (this.Stack.Count >= 1)
					{
						result = this.Stack.Pop();
					}
				}
			}
			return result;
		}
	}
}
