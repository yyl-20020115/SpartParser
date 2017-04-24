namespace Spart.Demo
{
    using Spart.Parsers.NonTerminal;
    using Spart.Parsers;
    using Spart.Scanners;
    using System.Collections.Generic;

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
            this.integer.Parser = (+Prims.Digit).WithAction(
				(parser, args) =>
				{
					if (!double.TryParse(args.Value, out double N))
					{
						N = double.NaN;
					}
					this.Stack.Push(
						(new Experssion
							{
								Value = args.Value,
							}
						,N)
						);
				}
			);

			this.group.Parser = '(' + this.expression + ')';

			this.factor.Parser = this.group | this.integer;

			void MathFunction(string op)
			{
				if (this.Stack.Count >= 2 && !string.IsNullOrEmpty(op))
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
				('*' + factor).WithAction((parser, args) => MathFunction("*"))
				|
				('/' + factor).WithAction((parser, args) => MathFunction("/"))
			);
            this.expression.Parser = this.term + ~(
				('+' + term).WithAction((parser, args) => MathFunction("+"))
				|
				('-' + term).WithAction((parser, args) => MathFunction("-"))
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
