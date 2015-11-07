using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Xml;
using HMS.Tools;
using HMS.DataVirtualization;

namespace HMS.Managers
{
    class FileManager // singleton
    {

        private static FileManager _fileManager; // instance

        public static FileManager instance
        {
            get
            {
                if (_fileManager == null)
                {
                    _fileManager = new FileManager();
                    return _fileManager;
                }
                return _fileManager;
            }
        }

        public void createFile(String fileName)
        {
            FileInfo file = new FileInfo(fileName);
            if(!file.Exists) {
                file.Create();
            } else {
               Debug.WriteLine("Такой файл уже сущетсвует!");
                //MessageBox.Show("Такой файл уже сущетсвует!");
            }
        }

        public void deleteFile(String fileName) {
            FileInfo file = new FileInfo(fileName);
            if (file.Exists)
            {
                file.Delete();
            }
            else
            {
                Debug.WriteLine("Файла с таким именем не существует");
                //MessageBox.Show("Файла с таким именем не существует");
            }

        }

        public void addTextInFile(String fileName, String text) {
            StreamWriter writeText; // класс для записи в файл
            FileInfo file = new FileInfo(fileName);
            writeText = file.AppendText();
            writeText.WriteLine(text);
            writeText.Close();
        }

        public String readFromFile(String fileName)
        {
            StreamReader streamReader = new StreamReader(fileName); //Открываем файл для чтения
            string  temp = "";

            while (!streamReader.EndOfStream) // до конца файла
            {
                temp += streamReader.ReadLine(); 
            }

            return temp;
        }

        //public Queue<ArticleObject> loadArticlesQueueFromXml(String xmlFile)
        //{
        //    Queue<ArticleObject> articles = new Queue<ArticleObject>();
        //    XmlDocument xDoc = new XmlDocument();
        //    xDoc.Load(xmlFile);
        //    XmlElement xRoot = xDoc.DocumentElement;
        //    foreach (XmlNode xNode in xRoot)
        //    {
        //        ArticleObject article = new ArticleObject();
        //        if (xNode.Attributes.Count > 0)
        //        {
        //            XmlNode attr = xNode.Attributes.GetNamedItem("name");
        //            if (attr != null)
        //                article.name = attr.Value;
        //        }
        //
        //        foreach (XmlNode childNode in xNode.ChildNodes)
        //        {
        //            switch (childNode.Name)
        //            {
        //                case "file_name":
        //                    article.fileName = childNode.InnerText;
        //                    break;
        //                case "articlenum":
        //                    article.article = childNode.InnerText;
        //                    break;
        //                case "min_diameter":
        //                    article.minDiameter = Converter.toDouble(childNode.InnerText);
        //                    break;
        //                case "max_diameter":
        //                    article.maxDiameter = Converter.toDouble(childNode.InnerText);
        //                    break;
        //                case "min_height":
        //                    article.minHeight = Converter.toDouble(childNode.InnerText);
        //                    break;
        //                case "max_height":
        //                    article.maxHeight = Converter.toDouble(childNode.InnerText);
        //                    break;
        //                case "min_sheight":
        //                    article.minSeamerHeight = Converter.toDouble(childNode.InnerText);
        //                    break;
        //                case "max_sheight":
        //                    article.maxSeamerHeight = Converter.toDouble(childNode.InnerText);
        //                    break;
        //                default:
        //                    break;
        //            }
        //        }
        //        articles.Enqueue(article);
        //    }
        //    return articles;
        //}

        //public void addArticleInXml(String fileName, ArticleObject obj)
        //{
        //    XmlDocument xDoc = new XmlDocument();
        //    xDoc.Load(fileName);
        //    XmlElement xRoot = xDoc.DocumentElement;
        //    XmlElement articleElem = xDoc.CreateElement("article");
        //
        //    // make attribute
        //    XmlAttribute nameAttr = xDoc.CreateAttribute("name");
        //    // make elements
        //    XmlElement fileNameElem = xDoc.CreateElement("file_name");
        //    XmlElement articlenumElem = xDoc.CreateElement("articlenum");
        //    XmlElement minDiameterElem = xDoc.CreateElement("min_diameter");
        //    XmlElement maxDiameterElem = xDoc.CreateElement("max_diameter");
        //    XmlElement minHeightElem = xDoc.CreateElement("min_height");
        //    XmlElement maxHeightElem = xDoc.CreateElement("max_height");
        //    XmlElement minSHeightElem = xDoc.CreateElement("min_sheight");
        //    XmlElement maxSHeightElem = xDoc.CreateElement("max_sheight");
        //    // make text values
        //    XmlText nameAttrText = xDoc.CreateTextNode(obj.name);
        //    XmlText fileNameText = xDoc.CreateTextNode(obj.fileName);
        //    XmlText articlenumText = xDoc.CreateTextNode(obj.article);
        //    XmlText minDiameterText = xDoc.CreateTextNode(obj.minDiameter.ToString());
        //    XmlText maxDiameterText = xDoc.CreateTextNode(obj.maxDiameter.ToString());
        //    XmlText minHeightText = xDoc.CreateTextNode(obj.minHeight.ToString());
        //    XmlText maxHeightText = xDoc.CreateTextNode(obj.maxHeight.ToString());
        //    XmlText minSHeightText = xDoc.CreateTextNode(obj.minSeamerHeight.ToString());
        //    XmlText maxSHeightText = xDoc.CreateTextNode(obj.maxSeamerHeight.ToString());
        //    // push text in nodes
        //    nameAttr.AppendChild(nameAttrText);
        //    fileNameElem.AppendChild(fileNameText);
        //    articlenumElem.AppendChild(articlenumText);
        //    minDiameterElem.AppendChild(minDiameterText);
        //    maxDiameterElem.AppendChild(maxDiameterText);
        //    minHeightElem.AppendChild(minHeightText); 
        //    maxHeightElem.AppendChild(maxHeightText);
        //    minSHeightElem.AppendChild(minSHeightText);
        //    maxSHeightElem.AppendChild(maxSHeightText);
        //    //push nodes in article elem
        //    articleElem.Attributes.Append(nameAttr);
        //    articleElem.AppendChild(fileNameElem);
        //    articleElem.AppendChild(articlenumElem);
        //    articleElem.AppendChild(minDiameterElem);
        //    articleElem.AppendChild(maxDiameterElem);
        //    articleElem.AppendChild(minHeightElem);
        //    articleElem.AppendChild(maxHeightElem);
        //    articleElem.AppendChild(minSHeightElem);
        //    articleElem.AppendChild(maxSHeightElem);
        //
        //    xRoot.AppendChild(articleElem);
        //
        //    xDoc.Save(fileName);
        //}

        public void deletenode()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load("users.xml");
            XmlElement xRoot = xDoc.DocumentElement;

            XmlNode firstNode = xRoot.FirstChild;
            xRoot.RemoveChild(firstNode);
            xDoc.Save("users.xml");
        }
    }
}

//ARArticleObject article = new ARArticleObject(nameTextBox.Text, articleTextBox.Text, Converter.toDouble(minDTextBox.Text), Converter.toDouble(maxDTextBox.Text),
            //    Converter.toDouble(minHTextBox.Text), Converter.toDouble(maxHTextBox.Text), Converter.toDouble(minSHTextBox.Text), Converter.toDouble(maxSHTextBox.Text));
            
            //ARFileManager.instance.addArticleInXml("myxml.xml", article);
            //foreach (ARArticleObject article in ARDataManager.instance.articles)
            //{
            //    Debug.WriteLine(article.name);
            //    Debug.WriteLine(article.article);
            //    Debug.WriteLine(article.fileName);
            //    Debug.WriteLine(article.minDiameter.ToString());
            //    Debug.WriteLine(article.maxDiameter.ToString());
            //    Debug.WriteLine(article.minHeight.ToString());
            //    Debug.WriteLine(article.maxHeight.ToString());
            //    Debug.WriteLine(article.minSeamerHeight.ToString());
            //    Debug.WriteLine(article.maxSeamerHeight.ToString());
            //}



/* <!-- Примечание -->
 *  
 * 
 * 
 * 
 * 
 */ 
