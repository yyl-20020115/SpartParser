namespace Spart.Demo
{
    using Spart.Parsers.NonTerminal;
    using Spart.Parsers;
    using Spart.Scanners;
    using Spart.Actions;
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

        protected Parser add = null;
        protected Parser sub = null;
        protected Parser mul = null;
        protected Parser div = null;
        protected Parser digits = null;

        protected Stack<double> CalculationStack = new Stack<double>();
        protected Stack<Experssion> ExpressionStack = new Stack<Experssion>();

        /// <summary>
        /// A very simple calculator parser
        /// </summary>
        public Calculator()
        {
            (this.add = '+' + term).Action += new ActionHandler(
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
            );

            (this.sub = '-' + term).Action += new ActionHandler(
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
            );

            (this.mul = '*' + factor).Action += new ActionHandler(
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
            );

            (this.div = '/' + factor).Action += new ActionHandler(
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
            );

            (this.digits = +Prims.Digit).Action += new ActionHandler(
                (parser, args) =>
                {
                    this.ExpressionStack.Push(
                        new Experssion
                        {
                            Value = args.Value,
                        }
                        );

                    if(!double.TryParse(args.Value, out double v))
                    {
                        v = double.NaN;
                    }
                    this.CalculationStack.Push(v);
                }
            );

            this.integer.Parser = digits;
            this.group.Parser = '(' + this.expression + ')';
            this.factor.Parser = this.group | this.integer;
            this.term.Parser = this.factor + ~(this.mul | this.div);
            this.expression.Parser = this.term + ~(this.add | this.mul);
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
        public virtual double Calculate(string text)
        {
            double result = double.NaN;

            if (!string.IsNullOrEmpty(text))
            {
                ParserMatch m = this.InternalParse(text);

                if (m.Success)
                {
                    if (this.CalculationStack.Count >= 1)
                    {
                        result = this.CalculationStack.Pop();
                    }
                }
            }
            return result;
        }
        public virtual Experssion Parse(string text)
        {
            Experssion result = null;

            if (!string.IsNullOrEmpty(text))
            {
                ParserMatch m = this.InternalParse(text);

                if (m.Success)
                {
                    if (this.ExpressionStack.Count >= 1)
                    {
                        result = this.ExpressionStack.Pop();
                    }
                }
            }
            return result;
        }
    }
}
