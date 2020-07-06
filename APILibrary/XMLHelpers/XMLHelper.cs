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
    /// <summary>�Զ��������xml�ļ�����</summary>
    public class XMLHelper
    {
        /// <summary>XML�ĵ�</summary>
        private System.Xml.XmlDocument _mXmlDoc = new System.Xml.XmlDocument();
        /// <summary>XML�ļ�·��</summary>
        private string XmlFilePath = "";
        /// <summary>
        /// XML�����ĵ���·��
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
        /// XML�ĵ��������캯��
        /// </summary>
        /// <param name="strFilePath">XML�ļ�·��</param>
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
        /// ����xml�����汾��Ϣ
        /// </summary>
        private void CreateXML(string filePath)
        {
            //����xml�����汾��Ϣ
            XmlDocument xml = new XmlDocument();
            xml.AppendChild(xml.CreateXmlDeclaration("1.0", "gb2312", null));
            var el = xml.CreateElement("SystemInfo");
            xml.AppendChild(el);
            xml.Save(filePath);
        }
        
        
        //$--��ʾ������
        /// <summary>
        /// ȡ��xml�ĵ�Ԫ��ֵ 
        /// </summary>
        /// <param name="node">�ڵ�</param>
        /// <param name="element">Ԫ����</param>
        /// <returns>Ԫ��ֵ �ַ���   </returns>
        public string GetElement(string node, string element)
        {
            try
            {
                System.Xml.XmlNode mXmlNode = _mXmlDoc.SelectSingleNode("//" + node);// //Խ���м�ڵ㣬��ѯ����
                if (mXmlNode != null)
                {
                    //������ 
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

        /// <summary>����Ԫ��ֵ</summary>
        /// <param name="node">�ڵ�����</param>
        /// <param name="element">Ԫ����</param>
        /// <param name="val">Ԫ��ֵ</param>
        /// <returns>True--����ɹ� False--����ʧ�� </returns>
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

        /// <summary>�ж��ĵ��Ƿ����������</summary>
        /// <returns>treu������ˣ�false:δ���</returns>
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
        /// ��ӽڵ㣬������
        /// </summary>
        /// <param name="xml">xml�ĵ�</param>
        /// <param name="nodeName">�ڵ���</param>
        /// <returns></returns>
        public XmlNode AddNode(XmlDocument xml, string nodeName)
        {
            XmlNode newNode = xml.CreateNode(XmlNodeType.Element, nodeName,null);
            //��ӽڵ������磺
            //newNode.Attributes.Append(AddAttribute(xml, "attrName", "attrValue"));
            return newNode;
        }


        /// <summary>
        /// ���һ������
        /// </summary>
        /// <param name="xml">xml�ĵ�</param>
        /// <param name="attributeName">������</param>
        /// <param name="attributeValue">����ֵ</param>
        /// <returns>��������</returns>
        public XmlAttribute AddAttribute(XmlDocument xml, string attributeName, string attributeValue)
        {
            XmlAttribute xmlAttr = xml.CreateAttribute(attributeName);
            xmlAttr.Value = attributeValue;
            return xmlAttr;
        }
    }

}
