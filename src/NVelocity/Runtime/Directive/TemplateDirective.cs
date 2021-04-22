using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using NVelocity.Context;
using NVelocity.Runtime.Parser.Node;
using NVelocity.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace NVelocity.Runtime.Directive
{
	public class TemplateDirective : Directive
	{
		private readonly TemplateProcess _templateProcess;

		public TemplateDirective()
		{
			_templateProcess = new TemplateProcess();
		}

		public override string Name { get => "template"; set => throw new NotSupportedException(); }

		public override DirectiveType Type { get => DirectiveType.LINE; }

		public override bool Render(IInternalContextAdapter context, TextWriter writer, INode node)
		{
			var myWriter = new StringWriter();

			var child = node?.GetChild(0);

			if(child != null)
			{
				var name = child.Value(context).ToString();

				var html = _templateProcess.GetTemplate(name).Result;

				var template = new StringTemplate(html);

				template.runtimeServices = runtimeServices;

				if (template.Process())
					((SimpleNode)template.Data).Render(context, myWriter);

				writer.WriteLine(myWriter.ToString());

				return true;
			}

			return false;
		}
	}
}
