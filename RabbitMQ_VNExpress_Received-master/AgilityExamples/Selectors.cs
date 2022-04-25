using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgilityExamples
{
    public class Selectors
    {
        public  void SelectNode()
        {
            var html =
        @"<TD class=texte width=""50%"">
			<DIV align=right>Name :<B> </B></DIV>
		</TD>
		<TD width=""50%"">
    		<INPUT class=box value=John maxLength=16 size=16 name=user_name>
    		<INPUT class=box value=Tony maxLength=16 size=16 name=user_name>
    		<INPUT class=box value=Jams maxLength=16 size=16 name=user_name>
		</TD>
		<TR vAlign=center>";

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var htmlNodes = htmlDoc.DocumentNode.SelectNodes("//td/input");

            foreach (var node in htmlNodes)
            {

                Console.WriteLine(node.Attributes["value"].Value);
            }

            //Select first node
            /* var html =
         @"<TD class=texte width=""50%"">
             <DIV align=right>Name :<B> </B></DIV>
         </TD>
         <TD width=""50%"">
             <INPUT class=box value=John maxLength=16 size=16 name=user_name>
             <INPUT class=box value=Tony maxLength=16 size=16 name=user_name>
             <INPUT class=box value=Jams maxLength=16 size=16 name=user_name>
         </TD>
         <TR vAlign=center>";

             var htmlDoc = new HtmlDocument();
             htmlDoc.LoadHtml(html);

             string name = htmlDoc.DocumentNode
                             .SelectNodes("//td/input")
                             .First()
                             .Attributes["value"].Value;

             Console.WriteLine(name);*/
        }

        public  void SelectSingleNode()
        {
            var html =
        @"<TD class=texte width=""50%"">
			<DIV align=right>Name :<B> </B></DIV>
		</TD>
		<TD width=""50%"">
    		<INPUT class=box value=John maxLength=16 size=16 name=user_name>
    		<INPUT class=box value=Tony maxLength=16 size=16 name=user_name>
    		<INPUT class=box value=Jams maxLength=16 size=16 name=user_name>
		</TD>
		<TR vAlign=center>";

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            string name = htmlDoc.DocumentNode
                .SelectSingleNode("//td/input")
                .Attributes["value"].Value;

            Console.WriteLine(name);//return John
        }
    }
}
