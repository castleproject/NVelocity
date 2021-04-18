using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NVelocity.Context;
using NVelocity.Runtime.Parser.Node;

namespace NVelocity.Runtime.Directive
{
	public class TestDirective : Directive
	{
		public override String Name
		{
			get { return "zinl"; }
			set { throw new NotSupportedException(); }
		}

		public override DirectiveType Type
		{
			get { return DirectiveType.LINE; }
		}

		public override bool Render(IInternalContextAdapter context, TextWriter writer, INode node)
		{
			writer.WriteLine("This is test directive");

			return true;
		}
	}
}
