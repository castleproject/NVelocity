using NVelocity.Context;
using NVelocity.Runtime.Parser.Node;
using NVelocity.Runtime.Resource;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NVelocity
{
	public class StringTemplate : Resource
	{
		private readonly Stream _streamData;
		public StringTemplate(string html)
		{
			var byteArr = System.Text.Encoding.UTF8.GetBytes(html);

			_streamData = new MemoryStream(byteArr);
			name = Guid.NewGuid().ToString();
			//Process();
		}

		public override bool Process()
		{
			if(_streamData != null)
			{
				try
				{
					StreamReader reader = new StreamReader(_streamData, System.Text.Encoding.GetEncoding(encoding));

					data = runtimeServices.Parse(reader, name);
					InitDocument();
					return true;
				}catch(System.Exception ex)
				{
					Console.WriteLine(ex.Message);
					return false;
				}
			}

			return false;
		}

		public void InitDocument()
		{
			// send an empty InternalContextAdapter down into the AST to initialize it
			InternalContextAdapterImpl internalContextAdapterImpl = new InternalContextAdapterImpl(new VelocityContext());

			try
			{
				// put the current template name on the stack
				internalContextAdapterImpl.PushCurrentTemplateName(name);

				// init the AST
				((SimpleNode)data).Init(internalContextAdapterImpl, runtimeServices);
			}
			finally
			{
				// in case something blows up...
				// pull it off for completeness
				internalContextAdapterImpl.PopCurrentTemplateName();
			}
		}
	}
}
