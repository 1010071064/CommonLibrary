using System.Data.SqlClient;
using System.Data.Odbc;
using System.Data;
using System;
using System.IO;
using System.Net;
using System.Xml;
//using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Text;
//using ToolClass;

namespace APILibrary.XMLHelpers
{
    /// <summary>自定义对配置xml文件操作</summary>
    public class XMLHelper
    {
        /// <summary>XML文档</summary>
        private System.Xml.XmlDocument _mXmlDoc = new System.Xml.XmlDocument();
        /// <summary>XML文件路径</summary>
        private string XmlFilePath = "";
        /// <summary>
        /// XML配置文档的路径
        /// </summary>
        public static string XMLPath
        {
            get
            {
                //return System.Windows.Forms.Application.StartupPath + "\\LocalSetting.XML";
                return System.AppDomain.CurrentDomain.BaseDirectory + "LocalSetting.XML";
            }
        }
        /// <summary>
        /// XML文档操作构造函数
        /// </summary>
        /// <param name="strFilePath">XML文件路径</param>
        public XMLHelper(string strFilePath)
        {
            try
            {
                if (!File.Exists(strFilePath))
                {
                    FileStream fs = File.Create(strFilePath);
                    fs.Close();
                    //CreateXML(strFilePath);
                }
                this.XmlFilePath = strFilePath;
                _mXmlDoc.Load(XmlFilePath);
            }
            catch (Exception exp)
            {
                throw exp;//LogClass.Error("CSysXML", "CSysXML(string File)", exp);
            }
        }
        /// <summary>
        /// 创建xml声明版本信息
        /// </summary>
        private void CreateXML(string filePath)
        {
            //创建xml声明版本信息
            XmlDocument xml = new XmlDocument();
            xml.AppendChild(xml.CreateXmlDeclaration("1.0", "gb2312", null));
            var el = xml.CreateElement("SystemInfo");
            xml.AppendChild(el);
            xml.Save(filePath);
        }
        
        
        //$--表示出错误
        /// <summary>
        /// 取得xml文档元素值 
        /// </summary>
        /// <param name="node">节点</param>
        /// <param name="element">元素名</param>
        /// <returns>元素值 字符型   </returns>
        public string GetElement(string node, string element)
        {
            try
            {
                System.Xml.XmlNode mXmlNode = _mXmlDoc.SelectSingleNode("//" + node);// //越过中间节点，查询其子
                if (mXmlNode != null)
                {
                    //读数据 
                    System.Xml.XmlNode xmlNode = mXmlNode.SelectSingleNode(element);
                    if (xmlNode != null)
                    {
                        return xmlNode.InnerText.ToString();
                    }
                }
            }
            catch (Exception exp)
            {
                throw exp;//LogClass.Error("CSysXML", "GetElement", exp);
            }
            return "";
        }

        /// <summary>保存元素值</summary>
        /// <param name="node">节点名称</param>
        /// <param name="element">元素名</param>
        /// <param name="val">元素值</param>
        /// <returns>True--保存成功 False--保存失败 </returns>
        public bool SaveElement(string node, string element, string val)
        {
            try
            {
                if (!ExiteXmlDeclaration())
                {
                    XmlNode dec = _mXmlDoc.CreateXmlDeclaration("1.0", "gb2312", "");
                    _mXmlDoc.AppendChild(dec);
                }
                System.Xml.XmlNode xmlRoot = _mXmlDoc.SelectSingleNode("//" + "SystemIP");
                if (xmlRoot == null)
                {
                    xmlRoot = _mXmlDoc.CreateElement("", "SystemIP", "");
                    _mXmlDoc.AppendChild(xmlRoot);
                }
                System.Xml.XmlNode mXmlNode = xmlRoot.SelectSingleNode("//" + node);
                if (mXmlNode == null)
                {
                    mXmlNode = _mXmlDoc.CreateElement("", node, "");
                    xmlRoot.AppendChild(mXmlNode);
                }

                System.Xml.XmlNode xmlNodeNew = mXmlNode.SelectSingleNode(element);

                if (xmlNodeNew == null)
                {
                    xmlNodeNew = _mXmlDoc.CreateElement("", element, "");
                    mXmlNode.AppendChild(xmlNodeNew);
                }
                xmlNodeNew.InnerText = val;
                _mXmlDoc.Save(this.XmlFilePath);

                return true;
            }
            catch (Exception exp)
            {
                //LogClass.Error("SaveElement", exp);
                throw exp;
            }
        }

        /// <summary>判断文档是否添加了声明</summary>
        /// <returns>treu：添加了，false:未添加</returns>
        private bool ExiteXmlDeclaration()
        {
            if (_mXmlDoc != null)
            {
                if (_mXmlDoc.ChildNodes.Count > 0)
                {
                    System.Xml.XmlNode xmlRoot = _mXmlDoc.ChildNodes[0];
                    if (xmlRoot.NodeType == XmlNodeType.XmlDeclaration)
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        /// <summary>
        /// 添加节点，或属性
        /// </summary>
        /// <param name="xml">xml文档</param>
        /// <param name="nodeName">节点名</param>
        /// <returns></returns>
        public XmlNode AddNode(XmlDocument xml, string nodeName)
        {
            XmlNode newNode = xml.CreateNode(XmlNodeType.Element, nodeName,null);
            //添加节点属性如：
            //newNode.Attributes.Append(AddAttribute(xml, "attrName", "attrValue"));
            return newNode;
        }


        /// <summary>
        /// 添加一个属性
        /// </summary>
        /// <param name="xml">xml文档</param>
        /// <param name="attributeName">属性名</param>
        /// <param name="attributeValue">属性值</param>
        /// <returns>返回属性</returns>
        public XmlAttribute AddAttribute(XmlDocument xml, string attributeName, string attributeValue)
        {
            XmlAttribute xmlAttr = xml.CreateAttribute(attributeName);
            xmlAttr.Value = attributeValue;
            return xmlAttr;
        }
    }

}
