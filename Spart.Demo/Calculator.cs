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

        protected Stack<double> CalculationStack = new Stack<double>();
        protected Stack<Experssion> ExpressionStack = new Stack<Experssion>();

        /// <summary>
        /// A very simple calculator parser
        /// </summary>
        public Calculator()
        {
            this.integer.Parser = (+Prims.Digit).WithAction(
				(parser, args) =>
				{
					this.ExpressionStack.Push(
						new Experssion
						{
							Value = args.Value,
						}
						);

					if (!double.TryParse(args.Value, out double v))
					{
						v = double.NaN;
					}
					this.CalculationStack.Push(v);
				}
			);

			this.group.Parser = '(' + this.expression + ')';

			this.factor.Parser = this.group | this.integer;

            this.term.Parser = this.factor + ~(
				('*' + factor).WithAction(
					(parser, args) =>
					{
						if (this.ExpressionStack.Count >= 2)
						{
							this.ExpressionStack.Push(
								new Experssion
								{
									Right = this.ExpressionStack.Pop(),
									Left = this.ExpressionStack.Pop(),
									Operation = "*"
								}
								);
						}

						if (this.CalculationStack.Count >= 2)
						{
							double y = this.CalculationStack.Pop();
							double x = this.CalculationStack.Pop();
							double z = x * y;
							this.CalculationStack.Push(z);
						}
					}
				)
				|
				('/' + factor).WithAction(
					(parser, args) =>
					{
						if (this.ExpressionStack.Count >= 2)
						{
							this.ExpressionStack.Push(
								new Experssion
								{
									Right = this.ExpressionStack.Pop(),
									Left = this.ExpressionStack.Pop(),
									Operation = "/"
								}
								);
						}

						if (this.CalculationStack.Count >= 2)
						{
							double y = this.CalculationStack.Pop();
							double x = this.CalculationStack.Pop();
							double z = x / y;
							this.CalculationStack.Push(z);
						}
					}
				)
			);
            this.expression.Parser = this.term + ~(
				('+' + term).WithAction(
					(parser, args) =>
					{
						if (this.ExpressionStack.Count >= 2)
						{
							this.ExpressionStack.Push(
								new Experssion
								{
									Right = this.ExpressionStack.Pop(),
									Left = this.ExpressionStack.Pop(),
									Operation = "+"
								}
								);
						}
						if (this.CalculationStack.Count >= 2)
						{
							double y = this.CalculationStack.Pop();
							double x = this.CalculationStack.Pop();
							double z = x + y;
							this.CalculationStack.Push(z);
						}
					}
				) 
				|
				('-' + term).WithAction(
					(parser, args) =>
					{
						if (this.ExpressionStack.Count >= 2)
						{
							this.ExpressionStack.Push(
								new Experssion
								{
									Right = this.ExpressionStack.Pop(),
									Left = this.ExpressionStack.Pop(),
									Operation = "-"
								}
								);
						}
						if (this.CalculationStack.Count >= 2)
						{
							double y = this.CalculationStack.Pop();
							double x = this.CalculationStack.Pop();
							double z = x - y;
							this.CalculationStack.Push(z);
						}
					}
				)
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
        public virtual (double n, Experssion e) Calculate(string text)
        {
            double n = double.NaN;

            Experssion e = null;

            if (!string.IsNullOrEmpty(text))
            {
                ParserMatch m = this.InternalParse(text);

                if (m.Success)
                {
                    if (this.CalculationStack.Count >= 1)
                    {
                        n = this.CalculationStack.Pop();
                    }
                    if (this.ExpressionStack.Count >= 1)
                    {
                        e = this.ExpressionStack.Pop();
                    }

                }
            }
            return (n,e);
        }
    }
}
