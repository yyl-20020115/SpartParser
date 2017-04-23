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

        protected Stack<double> Stack = new Stack<double>();

        /// <summary>
        /// A very simple calculator parser
        /// </summary>
        public Calculator()
        {
            // creating sub parsers
            add = '+' + term;
            // attaching semantic action
            add.Action += new ActionHandler(
                (parser, args) =>
                {
                    double y = this.Stack.Pop();
                    double x = this.Stack.Pop();
                    double z = x + y;
                    this.Stack.Push(z);
                }
            );

            sub = '-' + term;
            sub.Action += new ActionHandler(
                (parser, args) =>
                {
                    double y = this.Stack.Pop();
                    double x = this.Stack.Pop();
                    double z = x - y;
                    this.Stack.Push(z);
                }
            );

            mul = '*' + factor;
            mul.Action += new ActionHandler(
                (parser, args) =>
                {
                    double y = this.Stack.Pop();
                    double x = this.Stack.Pop();
                    double z = x * y;
                    this.Stack.Push(z);
                }
            );

            div = '/' + factor;
            div.Action += new ActionHandler(
                (parser, args) =>
                {
                    double y = this.Stack.Pop();
                    double x = this.Stack.Pop();
                    double z = x / y;
                    this.Stack.Push(z);
                }
            );

            // assigning parsers to rules
            this.integer.Parser = +Prims.Digit;
            this.integer.Action += new ActionHandler(
                (parser, args) =>
                {
                    if(double.TryParse(args.Value, out double v))
                    {
                        this.Stack.Push(v);
                    }
                }
            );

            this.group.Parser = '(' + expression + ')';
            this.group.Action += new ActionHandler(
                (parser, args) =>
                {

                }
            );

            this.factor.Parser = group | integer;
            this.factor.Action += new ActionHandler(
                (parser, args) =>
                {

                }
            );

            this.term.Parser = factor + ~(mul | div);
            this.term.Action += new ActionHandler(
                (parser, args) =>
                {

                }
            );
            this.expression.Parser = term + ~(add | mul);
            this.expression.Action += new ActionHandler(
                (parser, args) =>
                {
                }
            );
        }

        /// <summary>
        /// Parse a string and return parse match
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public virtual ParserMatch Parse(string s)
        {
            ParserMatch m = this.expression.Parse(new StringScanner(s));

            if (m.Success)
            {
                double z = this.Stack.Pop();

                Console.WriteLine(z);
            }
            return m;
        }
    }
}
