using System;

namespace Spart.Demo
{
    using Spart.Parsers.NonTerminal;
    using Spart.Parsers;
    using Spart.Scanners;
    using Spart.Actions;

    public class Calculator
    {
        Rule group;
        Rule term;
        Rule factor;
        Rule expression;
        Rule integer;

        /// <summary>
        /// A very simple calculator parser
        /// </summary>
        public Calculator()
        {
            // creating rules and assigning names (for debugging)
            group = new Rule()
            {
                ID = "group"
            };
            term = new Rule()
            {
                ID = "term"
            };
            factor = new Rule()
            {
                ID = "factor"
            };
            expression = new Rule()
            {
                ID = "expression"
            };
            integer = new Rule()
            {
                ID = "integer"
            };

            // creating sub parsers
            Parser add = Ops.Seq('+', term);
            // attaching semantic action
            add.Act += new ActionHandler(this.Add);
            Parser sub = Ops.Seq('-', term);
            sub.Act += new ActionHandler(this.Sub);
            Parser mult = Ops.Seq('*', factor);
            mult.Act += new ActionHandler(this.Mult);
            Parser div = Ops.Seq('/', factor);
            div.Act += new ActionHandler(this.Div);

            // assigning parsers to rules
            integer.Parser = Prims.Digit;

            group.Parser = Ops.Seq('(', Ops.Seq(expression, ')'));
            factor.Parser = group | integer;
            term.Parser = Ops.Seq(factor, Ops.Klenee(mult | div));
            expression.Parser = Ops.Seq(term, Ops.Klenee(add | mult));
        }

        /// <summary>
        /// Parse a string and return parse match
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public ParserMatch Parse(string s)
        {
            return expression.Parse(new StringScanner(s));
        }

        #region Semantic Actions
        public void Add(object sender, ActionEventArgs args)
        {
            Console.Out.WriteLine("add");
        }
        public void Sub(object sender, ActionEventArgs args)
        {
            Console.Out.WriteLine("sub");
        } 
        public void Mult(object sender, ActionEventArgs args)
        {
            Console.Out.WriteLine("mult");
        }
        public void Div(object sender, ActionEventArgs args)
        {
            Console.Out.WriteLine("div");
        }
        #endregion
    }
}
