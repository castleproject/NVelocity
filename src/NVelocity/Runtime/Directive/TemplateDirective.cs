using NVelocity.Context;
using NVelocity.Runtime.Parser.Node;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NVelocity.Runtime.Directive
{
	public class TemplateDirective : Directive
	{
		public override string Name { get => "template"; set => throw new NotSupportedException(); }

		public override DirectiveType Type { get => DirectiveType.LINE; }

		public override bool Render(IInternalContextAdapter context, TextWriter writer, INode node)
		{
			var myWriter = new StringWriter();

			var html = node.GetChild(0).Value(context).ToString();

			var template = new StringTemplate(html);

			template.runtimeServices = runtimeServices;

			if(template.Process())
				((SimpleNode)template.Data).Render(context, myWriter);

			writer.WriteLine(myWriter.ToString());

			return true;
		}
	}
}
