// Copyright 2004-2010 Castle Project - http://www.castleproject.org/
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
namespace NVelocity.Test
{
	using System;
	using System.Collections;
	using System.IO;
	using App;
	using NUnit.Framework;

	/// <summary>
	/// Tests to make sure that the VelocityContext is functioning correctly
	/// </summary>
	[TestFixture]
	public class ContextTest
	{
		[Test]
		public void ParamArraySupport1()
		{
			VelocityContext c = new VelocityContext();
			c.Put("x", new Something());

			StringWriter sw = new StringWriter();

			VelocityEngine velocityEngine = new VelocityEngine();
			velocityEngine.Init();

			Boolean ok = false;

			ok = velocityEngine.Evaluate(c, sw,
			                             "ContextTest.CaseInsensitive",
			                             "$x.Print( \"aaa\" )");

			Assert.IsTrue(ok, "Evaluation returned failure");
			Assert.AreEqual("aaa", sw.ToString());

			sw = new StringWriter();

			ok = velocityEngine.Evaluate(c, sw,
			                             "ContextTest.CaseInsensitive",
			                             "$x.Contents()");

			Assert.IsTrue(ok, "Evaluation returned failure");
			Assert.AreEqual(string.Empty, sw.ToString());

			sw = new StringWriter();

			ok = velocityEngine.Evaluate(c, sw,
			                             "ContextTest.CaseInsensitive",
			                             "$x.Contents( \"x\" )");

			Assert.IsTrue(ok, "Evaluation returned failure");
			Assert.AreEqual("x", sw.ToString());

			sw = new StringWriter();

			ok = velocityEngine.Evaluate(c, sw,
			                             "ContextTest.CaseInsensitive",
			                             "$x.Contents( \"x\", \"y\" )");

			Assert.IsTrue(ok, "Evaluation returned failure");
			Assert.AreEqual("x,y", sw.ToString());
		}

		[Test]
		public void ParamArraySupport2()
		{
			VelocityContext c = new VelocityContext();
			c.Put("x", new Something2());

			StringWriter sw = new StringWriter();

			VelocityEngine velocityEngine = new VelocityEngine();
			velocityEngine.Init();

			Boolean ok = false;

			ok = velocityEngine.Evaluate(c, sw,
			                             "ContextTest.CaseInsensitive",
			                             "$x.Contents( \"hammett\", 1 )");

			Assert.IsTrue(ok, "Evaluation returned failure");
			Assert.AreEqual("hammett1", sw.ToString());

			sw = new StringWriter();

			ok = velocityEngine.Evaluate(c, sw,
			                             "ContextTest.CaseInsensitive",
			                             "$x.Contents( \"hammett\", 1, \"x\" )");

			Assert.IsTrue(ok, "Evaluation returned failure");
			Assert.AreEqual("hammett1x", sw.ToString());

			sw = new StringWriter();

			ok = velocityEngine.Evaluate(c, sw,
			                             "ContextTest.CaseInsensitive",
			                             "$x.Contents( \"hammett\", 1, \"x\", \"y\" )");

			Assert.IsTrue(ok, "Evaluation returned failure");
			Assert.AreEqual("hammett1x,y", sw.ToString());
		}

		[Test]
		public void ParamArraySupportMultipleCalls()
		{
			VelocityContext c = new VelocityContext();
			c.Put("x", new Something());

			StringWriter sw = new StringWriter();

			VelocityEngine ve = new VelocityEngine();
			ve.Init();

			Boolean ok = false;

			ok = ve.Evaluate(c, sw,
			                 "ContextTest.CaseInsensitive",
			                 "$x.Contents( \"x\", \"y\" )\r\n$x.Contents( \"w\", \"z\" )\r\n$x.Contents( \"j\", \"f\", \"a\" )");

			Assert.IsTrue(ok, "Evaluation returned failure");
			Assert.AreEqual("x,y\r\nw,z\r\nj,f,a", sw.ToString());

			sw = new StringWriter();

			ok = ve.Evaluate(c, sw,
			                 "ContextTest.CaseInsensitive",
			                 "$x.Contents( \"x\", \"y\" )\r\n$x.Contents( \"w\", \"z\" )\r\n$x.Contents( \"j\", \"f\", \"a\" )");

			Assert.IsTrue(ok, "Evaluation returned failure");
			Assert.AreEqual("x,y\r\nw,z\r\nj,f,a", sw.ToString());
		}

		[Test]
		public void ParamArraySupportAndForEach()
		{
			ArrayList items = new ArrayList();

			items.Add("a");
			items.Add("b");
			items.Add("c");
			items.Add("d");

			VelocityContext c = new VelocityContext();
			c.Put("x", new Something());
			c.Put("items", items);

			StringWriter sw = new StringWriter();

			VelocityEngine ve = new VelocityEngine();
			ve.Init();

			Boolean ok = false;

			ok = ve.Evaluate(c, sw,
			                 "ContextTest.CaseInsensitive",
			                 "#foreach( $item in $items )\r\n$x.Contents( \"x\", \"y\" )\r\n#end\r\n");

			Assert.IsTrue(ok, "Evaluation returned failure");
			Assert.AreEqual("x,y\r\nx,y\r\nx,y\r\nx,y\r\n", sw.ToString());
		}

		[Test]
		public void ParamArraySupportAndForEach2()
		{
			ArrayList items = new ArrayList();

			items.Add("a");
			items.Add("b");
			items.Add("c");

			VelocityContext c = new VelocityContext();
			c.Put("x", new Something());
			c.Put("items", items);

			StringWriter sw = new StringWriter();

			VelocityEngine velocityEngine = new VelocityEngine();
			velocityEngine.Init();

			Boolean ok = false;

			ok = velocityEngine.Evaluate(c, sw,
			                             "ContextTest.CaseInsensitive",
			                             "#foreach( $item in $items )\r\n" +
			                             "#if($item == \"a\")\r\n $x.Contents( \"x\", \"y\" )#end\r\n" +
			                             "#if($item == \"b\")\r\n $x.Contents( \"x\" )#end\r\n" +
			                             "#if($item == \"c\")\r\n $x.Contents( \"c\", \"d\", \"e\" )#end\r\n" +
			                             "#end\r\n");

			Assert.IsTrue(ok, "Evaluation returned failure");
			Assert.AreEqual(" x,y x c,d,e", sw.ToString());
		}

		[Test]
		public void ForEachSimpleCase()
		{
			ArrayList items = new ArrayList();

			items.Add("a");
			items.Add("b");
			items.Add("c");

			VelocityContext c = new VelocityContext();
			c.Put("x", new Something2());
			c.Put("items", items);
			c.Put("d1", new DateTime(2005, 7, 16));
			c.Put("d2", new DateTime(2005, 7, 17));
			c.Put("d3", new DateTime(2005, 7, 18));

			StringWriter sw = new StringWriter();

			VelocityEngine ve = new VelocityEngine();
			ve.Init();

			Boolean ok = false;

			ok = ve.Evaluate(c, sw,
			                 "ContextTest.CaseInsensitive",
			                 "#foreach( $item in $items )\r\n" +
			                 "#if($item == \"a\")\r\n $x.FormatDate( $d1 )#end\r\n" +
			                 "#if($item == \"b\")\r\n $x.FormatDate( $d2 )#end\r\n" +
			                 "#if($item == \"c\")\r\n $x.FormatDate( $d3 )#end\r\n" +
			                 "#end\r\n");

			Assert.IsTrue(ok, "Evaluation returned failure");
			Assert.AreEqual(" 16 17 18", sw.ToString());
		}

		[Test]
		public void Hashtable1()
		{
			VelocityContext c = new VelocityContext();

			Hashtable x = new Hashtable();
			x.Add("item", "value1");

			c.Put("x", x);

			StringWriter sw = new StringWriter();

			VelocityEngine ve = new VelocityEngine();
			ve.Init();

			Boolean ok = false;

			ok = ve.Evaluate(c, sw,
			                 "ContextTest.CaseInsensitive",
			                 "$x.get_Item( \"item\" )");

			Assert.IsTrue(ok, "Evaluation returned failure");
			Assert.AreEqual("value1", sw.ToString());
		}

		[Test]
		public void CaseInsensitive()
		{
			// normal case sensitive context
			VelocityContext c = new VelocityContext();
			c.Put("firstName", "Cort");
			c.Put("LastName", "Schaefer");

			// verify the output, $lastName should not be resolved
			StringWriter sw = new StringWriter();

			VelocityEngine ve = new VelocityEngine();
			ve.Init();

			Boolean ok = ve.Evaluate(c, sw, "ContextTest.CaseInsensitive", "Hello $firstName $lastName");
			Assert.IsTrue(ok, "Evaluation returned failure");
			Assert.AreEqual("Hello Cort $lastName", sw.ToString());

			// create a context based on a case insensitive hashtable
			Hashtable ht = new Hashtable(StringComparer.OrdinalIgnoreCase);
			ht.Add("firstName", "Cort");
			ht.Add("LastName", "Schaefer");
			c = new VelocityContext(ht);

			// verify the output, $lastName should be resolved
			sw = new StringWriter();
			ok = ve.Evaluate(c, sw, "ContextTest.CaseInsensitive", "Hello $firstName $lastName");
			Assert.IsTrue(ok, "Evaluation returned failure");
			Assert.AreEqual("Hello Cort Schaefer", sw.ToString());

			// create a context based on a case insensitive hashtable, verify that stuff added to the context after it is created if found case insensitive
			ht = new Hashtable(StringComparer.OrdinalIgnoreCase);
			ht.Add("firstName", "Cort");
			c = new VelocityContext(ht);
			c.Put("LastName", "Schaefer");

			// verify the output, $lastName should be resolved
			sw = new StringWriter();
			ok = ve.Evaluate(c, sw, "ContextTest.CaseInsensitive", "Hello $firstName $lastName");
			Assert.IsTrue(ok, "Evaluation returned failure");
			Assert.AreEqual("Hello Cort Schaefer", sw.ToString());
		}

		[Test]
		public void PropertiesAreAlsoCaseInsensitive()
		{
			// normal case sensitive context
			VelocityContext c = new VelocityContext();
			c.Put("something", new Something());

			VelocityEngine ve = new VelocityEngine();
			ve.Init();

			// verify the output, $lastName should not be resolved
			StringWriter sw = new StringWriter();

			bool ok = ve.Evaluate(c, sw, string.Empty, "Hello $something.firstName");
			Assert.IsTrue(ok, "Evaluation returned failure");
			Assert.AreEqual("Hello hammett", sw.ToString());

			sw.GetStringBuilder().Length = 0;

			ok = ve.Evaluate(c, sw, string.Empty, "Hello $something.Firstname");
			Assert.IsTrue(ok, "Evaluation returned failure");
			Assert.AreEqual("Hello hammett", sw.ToString());

			sw.GetStringBuilder().Length = 0;

			ok = ve.Evaluate(c, sw, string.Empty, "Hello $something.firstname");
			Assert.IsTrue(ok, "Evaluation returned failure");
			Assert.AreEqual("Hello hammett", sw.ToString());
		}
	}

	public class Something
	{
		private string firstName = "hammett";
		private string middleNameInitial = "V";

		public String Print(String arg)
		{
			return arg;
		}

		public String Contents(params String[] args)
		{
			return String.Join(",", args);
		}

		public string FirstName
		{
			get { return firstName; }
			set { firstName = value; }
		}

		public string MiddleNameInitial
		{
			get { return middleNameInitial; }
			set { middleNameInitial = value; }
		}
	}

	public class Something2
	{
		public String FormatDate(DateTime dt)
		{
			return dt.Day.ToString();
		}

		public String Contents(String name, int age, params String[] args)
		{
			return name + age + String.Join(",", args);
		}
	}
}
