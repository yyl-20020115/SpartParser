using System;

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

        protected Stack<double> Stack = new Stack<double>();

        /// <summary>
        /// A very simple calculator parser
        /// </summary>
        public Calculator()
        {
            (this.add = '+' + term).Action += new ActionHandler(
                (parser, args) =>
                {
                    if (this.Stack.Count >= 2)
                    {
                        double y = this.Stack.Pop();
                        double x = this.Stack.Pop();
                        double z = x + y;
                        this.Stack.Push(z);
                    }
                }
            );

            (this.sub = '-' + term).Action += new ActionHandler(
                (parser, args) =>
                {
                    if (this.Stack.Count >= 2)
                    {
                        double y = this.Stack.Pop();
                        double x = this.Stack.Pop();
                        double z = x - y;
                        this.Stack.Push(z);
                    }
                }
            );

            (this.mul = '*' + factor).Action += new ActionHandler(
                (parser, args) =>
                {
                    if (this.Stack.Count >= 2)
                    {
                        double y = this.Stack.Pop();
                        double x = this.Stack.Pop();
                        double z = x * y;
                        this.Stack.Push(z);
                    }
                }
            );

            (this.div = '/' + factor).Action += new ActionHandler(
                (parser, args) =>
                {
                    if (this.Stack.Count >= 2)
                    {
                        double y = this.Stack.Pop();
                        double x = this.Stack.Pop();
                        double z = x / y;
                        this.Stack.Push(z);
                    }
                }
            );

            (this.digits = +Prims.Digit).Action += new ActionHandler(
                (parser, args) =>
                {
                    if(double.TryParse(args.Value, out double v))
                    {
                        this.Stack.Push(v);
                    }
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
        protected virtual ParserMatch Parse(string e)
        {
            return this.expression.Parse(new StringScanner(e));
        }
        public virtual double Calculate(string e)
        {
            double result = double.NaN;

            ParserMatch m = this.Parse(e);

            if (m.Success)
            {
                if (this.Stack.Count >= 1)
                {
                    result = this.Stack.Pop();
                }
            }
            return result;
        }
    }
}
