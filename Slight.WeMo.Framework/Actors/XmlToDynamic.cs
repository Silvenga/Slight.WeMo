
using System;
using System.Collections.Generic;
using System.Linq;

namespace Slight.WeMo.Framework.Actors
{
    using System.Dynamic;
    using System.Xml.Linq;

    // http://www.itdevspace.com/2012/07/parse-xml-to-dynamic-object-in-c.html
    public static class XmlToDynamic
    {
        public static void Parse(dynamic parent, XElement node)
        {
            if (node.HasElements)
            {
                if (node.Elements(node.Elements().First().Name.LocalName).Count() > 1)
                {
                    var item = new ExpandoObject();
                    var list = new List<dynamic>();
                    foreach (var element in node.Elements())
                    {
                        Parse(list, element);
                    }

                    AddProperty(item, node.Elements().First().Name.LocalName, list);
                    AddProperty(parent, node.Name.LocalName, item);
                }
                else
                {
                    var item = new ExpandoObject();

                    foreach (var attribute in node.Attributes())
                    {
                        AddProperty(item, attribute.Name.LocalName, attribute.Value.Trim());
                    }

                    foreach (var element in node.Elements())
                    {
                        Parse(item, element);
                    }

                    AddProperty(parent, node.Name.LocalName, item);
                }
            }
            else
            {
                AddProperty(parent, node.Name.LocalName, node.Value.Trim());
            }
        }

        private static void AddProperty(dynamic parent, string name, object value)
        {
            if (parent is List<dynamic>)
            {
                ((List<dynamic>) parent).Add(value);
            }
            else
            {
                (parent as IDictionary<String, object>)[name] = value;
            }
        }
    }
}
