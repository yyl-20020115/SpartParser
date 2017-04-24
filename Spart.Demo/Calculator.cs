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
			public string Value = null;
			public string Operation = null;
			public Experssion Left = null;
			public Experssion Right = null;
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
			this.integer.Parser = (+Prims.Digit).With(
				(parser, args) =>
				{
					string value = args?.Value;

					this.Stack.Push(
						(new Experssion
						{
							Value = value
						}
						, double.TryParse(value, out double N) ? N : double.NaN
						)
					);
				}
			);

			this.group.Parser = '(' + this.expression + ')';

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

					this.Stack.Push(
					(
						new Experssion
						{
							Left = X.E,
							Right = Y.E,
							Operation = op
						},
						z
						)
					);
				}
			}

			this.term.Parser = this.factor + ~(
				('*' + factor).With(DoMath)
				|
				('/' + factor).With(DoMath)
			);

			this.expression.Parser = this.term + ~(
				('+' + term).With(DoMath)
				|
				('-' + term).With(DoMath)
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
