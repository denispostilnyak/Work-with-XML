using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;

namespace lab_3
{
    public partial class Form1 : Form
    {
        public Form1()
        {

            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GetAllScientist();
        }
        public void GetAllScientist()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"C:\Users\Denis\source\repos\lab_3\lab_3\XMLFile1.xml");
          
            XmlNodeList elem = doc.SelectNodes("//Faculty");
           foreach(XmlNode e in elem)
            {
                XmlNodeList list1 = e.ChildNodes;
                foreach(XmlNode el in list1)
                {
                    addItems(el);
                }
            }
        }
        private void addItems(XmlNode n)
        {
            string Faculty = n.ParentNode.Attributes.GetNamedItem("FACULTY").Value;

            if (!comboBox1.Items.Contains(n.SelectSingleNode("@FullName").Value))
                comboBox1.Items.Add(n.SelectSingleNode("@FullName").Value);
            if (!comboBox2.Items.Contains(n.SelectSingleNode("@Department").Value))
                comboBox2.Items.Add(n.SelectSingleNode("@Department").Value);
            if (!comboBox3.Items.Contains(n.SelectSingleNode("@Field").Value))
                comboBox3.Items.Add(n.SelectSingleNode("@Field").Value);
            if (!comboBox4.Items.Contains(n.SelectSingleNode("@Chair").Value))
                comboBox4.Items.Add(n.SelectSingleNode("@Chair").Value);
            if (!comboBox5.Items.Contains(n.SelectSingleNode("@Laboratory").Value))
                comboBox5.Items.Add(n.SelectSingleNode("@Laboratory").Value);
            if (!comboBox6.Items.Contains(n.SelectSingleNode("@Position").Value))
                comboBox6.Items.Add(n.SelectSingleNode("@Position").Value);
            if (!comboBox7.Items.Contains(n.SelectSingleNode("@OnPositionFrom").Value))
                comboBox7.Items.Add(n.SelectSingleNode("@OnPositionFrom").Value);
            if (!comboBox8.Items.Contains(n.SelectSingleNode("@OnPositionTo").Value))
                comboBox8.Items.Add(n.SelectSingleNode("@OnPositionTo").Value);
            if (!comboBox9.Items.Contains(Faculty))
                comboBox9.Items.Add(Faculty);
        }

        private void Search_Button_Click(object sender, EventArgs e)
        {
            search();
        }
        private void search()
        {
            Results.Text = "";
            Scientists scientists = new Scientists();
            if (FullName_CheckBox.Checked)
                scientists.FullName = comboBox1.SelectedItem.ToString();
            if (Department_CheckBox.Checked)
                scientists.Department = comboBox2.SelectedItem.ToString();
            if (Field_CheckBox.Checked)
                scientists.Field = comboBox3.SelectedItem.ToString();
            if (Chair_CheckBox.Checked)
                scientists.Chair = comboBox4.SelectedItem.ToString();
            if (Laboratory_CheckBox.Checked)
                scientists.Laboratory = comboBox5.SelectedItem.ToString();
            if (Position_CheckBox.Checked)
                scientists.Position = comboBox6.SelectedItem.ToString();
            if (OnPositionFrom_CheckBox.Checked)
                scientists.OnPositionFrom = comboBox7.SelectedItem.ToString();
            if (OnPositionTo_checkBox.Checked)
                scientists.OnPositionTo = comboBox8.SelectedItem.ToString();
            if (Faculty_checkBox.Checked)
                scientists.Faculty = comboBox9.SelectedItem.ToString();
            IAnalystXMLStrategy analyst = new AnalystXMLDOMStrategy();
            if (DOM_radioButton.Checked)
                analyst = new AnalystXMLDOMStrategy();
            if (SAX_radioButton.Checked)
                analyst = new AnalystXMLSAXStrategy();
            if (LINQ_TO_XML_radioButton.Checked)
                analyst = new AnalystXMLLINQStrategy();

            Search search = new Search(analyst, scientists);
            List<Scientists> result = search.SearchAlgoritm();
            foreach(Scientists sc in result)
            {
                Results.Text += "Факультет: " + sc.Faculty + "\n";
                Results.Text += "П.І.П.: " + sc.FullName + "\n";
                Results.Text += "Департамент: " + sc.Department + "\n";
                Results.Text += "Відділення: " + sc.Field + "\n";
                Results.Text += "Кафедра: " + sc.Chair + "\n";
                Results.Text += "Лабораторія: " + sc.Laboratory + "\n";
                Results.Text += "Посада: " + sc.Position + "\n";
                Results.Text += "На посаді з: " + sc.OnPositionFrom + "\n";
                Results.Text += "На посаді до: " + sc.OnPositionTo + "\n";
                Results.Text += "\n\n\n";
            }
        }

        private void Clear_button_Click(object sender, EventArgs e)
        {
            Results.Text = "";
            FullName_CheckBox.Checked = false;
            Faculty_checkBox.Checked = false;
            Department_CheckBox.Checked = false;
            Field_CheckBox.Checked = false;
            Chair_CheckBox.Checked = false;
            Laboratory_CheckBox.Checked = false;
            Position_CheckBox.Checked = false;
            OnPositionFrom_CheckBox.Checked = false;
            OnPositionTo_checkBox.Checked = false;
            comboBox1.Text = null;
            comboBox2.Text = null;
            comboBox3.Text = null;
            comboBox4.Text = null;
            comboBox5.Text = null;
            comboBox6.Text = null;
            comboBox7.Text = null;
            comboBox8.Text = null;
            comboBox9.Text = null;
            DOM_radioButton.Checked = false;
            SAX_radioButton.Checked = false;
            LINQ_TO_XML_radioButton.Checked = false;
        }

        private void Transformation_button_Click(object sender, EventArgs e)
        {
            transform();
        }
        private void transform()
        {
            XslCompiledTransform xct = new XslCompiledTransform();
            xct.Load(@"E:\STUDING\Proga\C#\File.xsl");
            string FXML = @"C:\Users\Denis\source\repos\lab_3\lab_3\XMLFile1.xml";
            string FHTML = @"E:\STUDING\Proga\C#\File.html";
            xct.Transform(FXML, FHTML);
            MessageBox.Show("Done");
        }

        private void FullName_CheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
    public class Search
    {
        IAnalystXMLStrategy _analyst;
        Scientists _scientists;
        internal Search(IAnalystXMLStrategy analyst, Scientists scientists)
        {
            _analyst = analyst;
            _scientists = scientists;
        }
        public List<Scientists> SearchAlgoritm()
        {
            List<Scientists> result = new List<Scientists>();
            if(_analyst is AnalystXMLDOMStrategy)
            {
                result = _analyst.Search(_scientists);
            }
            if (_analyst is AnalystXMLSAXStrategy)
            {
                result = _analyst.Search(_scientists);
            }
            if (_analyst is AnalystXMLLINQStrategy)
            {
                result = _analyst.Search(_scientists);
            }
            return result;
        }
    }
    interface IAnalystXMLStrategy
    {
        List<Scientists> Search(Scientists scientists);
    }
    
    public class Scientists
    {
        public Scientists()
        {

        }
        
        public string FullName { get; set; }
        public string Department { get; set; }
        public string Field { get; set; }
        public string Chair { get; set; }
        public string Laboratory { get; set; }
        public string Position { get; set; }
        public string OnPositionFrom { get; set; }
        public string OnPositionTo { get; set; }
        public string Faculty { get; set; }
    }
   
    class AnalystXMLDOMStrategy:IAnalystXMLStrategy
    {
        public List<Scientists> Search(Scientists scientists)
        {
            List<Scientists> result = new List<Scientists>();
            XmlDocument doc = new XmlDocument();
            int l = 5;
            int k = 0;
            doc.Load(@"C:\Users\Denis\source\repos\lab_3\lab_3\XMLFile1.xml");
            XmlNode node = doc.DocumentElement;
            foreach(XmlNode nod in node.ChildNodes)
            {
                string FullName = "";
                string Department = "";
                string Field = "";
                string Chair = "";
                string Laboratory = "";
                string Position = "";
                string OnPositionFrom = "";
                string OnPositionTo = "";
                string Faculty = "";
                
                foreach(XmlAttribute nod2 in nod.Attributes)
                {
                    if (nod2.Name.Equals("FACULTY") && (nod2.Value.Equals(scientists.Faculty) || scientists.Faculty == null))
                        Faculty = nod2.Value;
                }

                foreach (XmlNode nod1 in nod.ChildNodes)
                {
                  
                    
                    foreach (XmlAttribute attribute in nod1.Attributes)
                    {

                        if (attribute.Name.Equals("FullName") && (attribute.Value.Equals(scientists.FullName) || scientists.FullName == null))
                        { FullName = attribute.Value; k++; }
                        if (attribute.Name.Equals("Department") && (attribute.Value.Equals(scientists.Department) || scientists.Department == null))
                        { Department = attribute.Value; k++; }
                        if (attribute.Name.Equals("Field") && (attribute.Value.Equals(scientists.Field) || scientists.Field == null))
                        { Field = attribute.Value; k++; }
                        if (attribute.Name.Equals("Chair") && (attribute.Value.Equals(scientists.Chair) || scientists.Chair == null))
                        { Chair = attribute.Value; k++; }
                        if (attribute.Name.Equals("Laboratory") && (attribute.Value.Equals(scientists.Laboratory) || scientists.Laboratory == null))
                        { Laboratory = attribute.Value; k++; }
                        if (attribute.Name.Equals("Position") && (attribute.Value.Equals(scientists.Position) || scientists.Position == null))
                        { Position = attribute.Value; k++; }
                        if (attribute.Name.Equals("OnPositionFrom") && (attribute.Value.Equals(scientists.OnPositionFrom) || scientists.OnPositionFrom == null))
                        { OnPositionFrom = attribute.Value; k++; }
                        if (attribute.Name.Equals("OnPositionTo") && (attribute.Value.Equals(scientists.OnPositionTo) || scientists.OnPositionTo == null))
                        { OnPositionTo = attribute.Value; k++; }
                        if (k == 0) { l = k; }
                        k = 0;
                    }

                    if (FullName != "" && Department != "" && Field != "" && Chair != "" && Laboratory != "" && Position != "" && OnPositionFrom != "" && OnPositionTo != "" && Faculty != "" && l != 0) 
                    {
                        Scientists myScientist = new Scientists();
                        myScientist.FullName = FullName;
                        myScientist.Department = Department;
                        myScientist.Field = Field;
                        myScientist.Chair = Chair;
                        myScientist.Laboratory = Laboratory;
                        myScientist.Position = Position;
                        myScientist.OnPositionFrom = OnPositionFrom;
                        myScientist.OnPositionTo = OnPositionTo;
                        myScientist.Faculty = Faculty;
                        result.Add(myScientist);
                    }
                    l = 5;
                }
            }
            return result;
        }
    }
    class AnalystXMLSAXStrategy:IAnalystXMLStrategy
    {
        public List<Scientists> Search(Scientists scientists)
        {
            
            List<Scientists> allresult = new List<Scientists>();
            XmlReader xmlreader = XmlReader.Create(@"C:\Users\Denis\source\repos\lab_3\lab_3\XMLFile1.xml");
            string Faculty = "";
            while (xmlreader.Read())
            {
                
                if (xmlreader.HasAttributes)
                {
                    if(xmlreader.AttributeCount==1)
                    {
                        xmlreader.MoveToNextAttribute();
                    }

                    if ((xmlreader.Name.Equals("FACULTY") && xmlreader.Value.Equals(scientists.Faculty)) || (scientists.Faculty == null && xmlreader.Name.Equals("FACULTY")) )
                    {
                        Faculty = "";
                        Faculty = xmlreader.Value;
                        xmlreader.Read();
                    }
                    else
                    if ((xmlreader.Value != scientists.Faculty) && xmlreader.Name.Equals("FACULTY") && allresult.Count > 0) 
                    {
                      return allresult;
                    }
                    while (xmlreader.MoveToNextAttribute())
                    {

                        string FullName = "";
                        string Department = "";
                        string Field = "";
                        string Chair = "";
                        string Laboratory = "";
                        string Position = "";
                        string OnPositionFrom = "";
                        string OnPositionTo = "";
                       
                            if (xmlreader.Name.Equals("FullName") && xmlreader.Value.Equals(scientists.FullName) || scientists.FullName == null)
                            {
                                FullName = xmlreader.Value;
                                xmlreader.MoveToNextAttribute();
                                if (xmlreader.Name.Equals("Department") && xmlreader.Value.Equals(scientists.Department) || scientists.Department == null)
                                {
                                    Department = xmlreader.Value;
                                    xmlreader.MoveToNextAttribute();
                                    if (xmlreader.Name.Equals("Field") && xmlreader.Value.Equals(scientists.Field) || scientists.Field == null)
                                    {
                                        Field = xmlreader.Value;
                                        xmlreader.MoveToNextAttribute();
                                        if (xmlreader.Name.Equals("Chair") && xmlreader.Value.Equals(scientists.Chair) || scientists.Chair == null)
                                        {
                                            Chair = xmlreader.Value;
                                            xmlreader.MoveToNextAttribute();
                                            if (xmlreader.Name.Equals("Laboratory") && xmlreader.Value.Equals(scientists.Laboratory) || scientists.Laboratory == null)
                                            {
                                                Laboratory = xmlreader.Value;
                                                xmlreader.MoveToNextAttribute();
                                                if (xmlreader.Name.Equals("Position") && xmlreader.Value.Equals(scientists.Position) || scientists.Position == null)
                                                {
                                                    Position = xmlreader.Value;
                                                    xmlreader.MoveToNextAttribute();
                                                    if (xmlreader.Name.Equals("OnPositionFrom") && xmlreader.Value.Equals(scientists.OnPositionFrom) || scientists.OnPositionFrom == null)
                                                    {
                                                        OnPositionFrom = xmlreader.Value;
                                                        xmlreader.MoveToNextAttribute();
                                                        if (xmlreader.Name.Equals("OnPositionTo") && xmlreader.Value.Equals(scientists.OnPositionTo) || scientists.OnPositionTo == null)
                                                        {
                                                            OnPositionTo = xmlreader.Value;

                                                        }

                                                    }

                                                }

                                            }

                                        }

                                    }

                                }
                            }

                        

                        if (FullName != "" && Department != "" && Field != "" && Chair != "" && Laboratory != "" && Position != "" && OnPositionFrom != "" && OnPositionTo != "" && Faculty != "")
                        {
                            Scientists myScientist = new Scientists();
                            myScientist.Faculty = Faculty;
                            myScientist.FullName = FullName;
                            myScientist.Department = Department;
                            myScientist.Field = Field;
                            myScientist.Chair = Chair;
                            myScientist.Laboratory = Laboratory;
                            myScientist.Position = Position;
                            myScientist.OnPositionFrom = OnPositionFrom;
                            myScientist.OnPositionTo = OnPositionTo;
                            allresult.Add(myScientist);
                        }

                    }

                    
                }
            }
            xmlreader.Close();
            return allresult;
        }

    }
    class AnalystXMLLINQStrategy : IAnalystXMLStrategy
    {
        public List<Scientists> Search(Scientists scientists)
        {
            List<Scientists> allResult = new List<Scientists>();
            var doc = XDocument.Load(@"C:\Users\Denis\source\repos\lab_3\lab_3\XMLFile1.xml");

            var result = from obj in doc.Descendants("Scientist")
                         where ((obj.Parent.Attribute("FACULTY").Value.Equals(scientists.Faculty) || scientists.Faculty == null) &&
                                (obj.Attribute("FullName").Value.Equals(scientists.FullName) || scientists.FullName == null) &&
                                (obj.Attribute("Department").Value.Equals(scientists.Department) || scientists.Department == null) &&
                                (obj.Attribute("Field").Value.Equals(scientists.Field) || scientists.Field == null) &&
                                (obj.Attribute("Chair").Value.Equals(scientists.Chair) || scientists.Chair == null) &&
                                (obj.Attribute("Laboratory").Value.Equals(scientists.Laboratory) || scientists.Laboratory == null) &&
                                (obj.Attribute("Position").Value.Equals(scientists.Position) || scientists.Position == null) &&
                                (obj.Attribute("OnPositionFrom").Value.Equals(scientists.OnPositionFrom) || scientists.OnPositionFrom == null) &&
                                (obj.Attribute("OnPositionTo").Value.Equals(scientists.OnPositionTo) || scientists.OnPositionTo == null)
                                )
                         select new
                         {
                             faculty = (string)obj.Parent.Attribute("FACULTY"),
                             fullname = (string)obj.Attribute("FullName"),
                             department = (string)obj.Attribute("Department"),
                             field = (string)obj.Attribute("Field"),
                             chair = (string)obj.Attribute("Chair"),
                             laboratory = (string)obj.Attribute("Laboratory"),
                             position = (string)obj.Attribute("Position"),
                             onpositionfrom = (string)obj.Attribute("OnPositionFrom"),
                             onpositionto = (string)obj.Attribute("OnPositionTo")
                         };
            foreach(var n in result)
            {
                Scientists myScientist = new Scientists();
                myScientist.Faculty = n.faculty;
                myScientist.FullName = n.fullname;
                myScientist.Department = n.department;
                myScientist.Field = n.field;
                myScientist.Chair = n.chair;
                myScientist.Laboratory = n.laboratory;
                myScientist.Position = n.position;
                myScientist.OnPositionFrom = n.onpositionfrom;
                myScientist.OnPositionTo = n.onpositionto;
                allResult.Add(myScientist);
            }
            return allResult;
        }
    }
   
}
