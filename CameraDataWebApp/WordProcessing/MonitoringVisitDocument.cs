using CameraDataWebApp.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using System.IO;
using DocumentFormat.OpenXml.Wordprocessing;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;
using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;


namespace CameraDataWebApp.WordProcessing
{
    //Tän pitäis palauttaa .docx asiakirja missä olisi valvontamuistio näppärästi valmiina 
    public class MonitoringVisitDocument
    {
        //private List<Observation> _observations;
        //private readonly ObservationContext _context;
        private MonitoringVisit _monitorigVisit;
        public string dataFolder;
        public string newFilePath;
        public string templateFolder;
        public MonitoringVisitDocument(MonitoringVisit monitoringVisit)//(List<Observation> observations)
        {
            _monitorigVisit = monitoringVisit;
            CheckDataFolder();
            CheckTemplateFolder();

        }

        public string GetMonitoringVisitDocument() //tän pitäis varmaan olla jokin wordprocessingdocument eikä void?
        {
            try
            {
                var OriginalFilePath = $"{templateFolder}Valvontamuistio.docx";
                newFilePath = $"{dataFolder}{_monitorigVisit.casenumber} {_monitorigVisit.visitname}.docx";
                //POISTAA UUDEN TIEDOSTON JOS SELLAINEN ON JO OLEMASSA
                if (File.Exists(newFilePath))
                {
                    File.Delete(newFilePath);
                }

                File.Copy(OriginalFilePath, newFilePath);

                using (WordprocessingDocument wordDocument =
            WordprocessingDocument.Open(newFilePath, true))
                {
                    //// Add a main document part.
                    MainDocumentPart mainPart = wordDocument.MainDocumentPart;
                    Table table = mainPart.Document.Descendants<Table>().Where(x => x.Descendants<Drawing>().Count() > 0).FirstOrDefault(); //Tää ei nyt toimi kun eka taulukko ei riitä

                    if (table != null && _monitorigVisit.observations != null)
                    {
                        int n = 1;
                        foreach (var observation in _monitorigVisit.observations)
                        {
                            TableRow tr1 = (TableRow)table.Descendants<TableRow>().FirstOrDefault().Clone();
                            List<TableCell> cells = tr1.Descendants<TableCell>().ToList();
                            TableCell tc1 = tr1.Descendants<TableCell>().FirstOrDefault();
                            TableCell tc2 = tr1.Descendants<TableCell>().LastOrDefault();

                            //KUVASOLU
                            ImagePart imagePart = mainPart.AddImagePart(ImagePartType.Jpeg);
                            using (FileStream stream = new FileStream(observation.imageuri, FileMode.Open))
                            {
                                imagePart.FeedData(stream);
                            }

                            Drawing drawing = tc1.Descendants<Drawing>().FirstOrDefault();
                            A.Blip blip = drawing.Descendants<A.Blip>().FirstOrDefault();
                            blip.Embed = mainPart.GetIdOfPart(imagePart);

                            ////tekstisolu
                            //Kuvanumero
                            
                            Bold bold = new Bold();
                            Paragraph paraPicturenumber = tc2.Descendants<Paragraph>().FirstOrDefault();

                            Run runPicturenumber = new Run(new RunProperties(new Bold()), new Text($"Kuva {n}"));
                            paraPicturenumber.Append(runPicturenumber);

                            n++;
                            //Havaintoteksti
                            Paragraph paraObservation = new Paragraph(new Run(new Text($"{observation.observation}")));
                            
                            tc2.Append(paraObservation);
                            //tr1.Append(tc12);

                            //kuva rivin lisäys
                            table.Append(tr1); 
                        }
                        TableRow trTemplate = table.Descendants<TableRow>().FirstOrDefault();
                        trTemplate.Remove();
                    }

                    wordDocument.Save();
                    wordDocument.Close();
                    return newFilePath;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "";
                throw;
                
            }


        }

        private void AddImageToBody(WordprocessingDocument wordDoc, string relationshipId, TableCell tc1)
        {
            // Define the reference of the image.
            var element =
                 new Drawing(
                     new DW.Inline(
                         new DW.Extent() { Cx = 990000L, Cy = 792000L },
                         new DW.EffectExtent()
                         {
                             LeftEdge = 0L,
                             TopEdge = 0L,
                             RightEdge = 0L,
                             BottomEdge = 0L
                         },
                         new DW.DocProperties()
                         {
                             Id = (UInt32Value)1U,
                             Name = "Picture 1"
                         },
                         new DW.NonVisualGraphicFrameDrawingProperties(
                             new A.GraphicFrameLocks() { NoChangeAspect = true }),
                         new A.Graphic(
                             new A.GraphicData(
                                 new PIC.Picture(
                                     new PIC.NonVisualPictureProperties(
                                         new PIC.NonVisualDrawingProperties()
                                         {
                                             Id = (UInt32Value)0U,
                                             Name = "New Bitmap Image.jpg"
                                         },
                                         new PIC.NonVisualPictureDrawingProperties()),
                                     new PIC.BlipFill(
                                         new A.Blip(
                                             new A.BlipExtensionList(
                                                 new A.BlipExtension()
                                                 {
                                                     Uri =
                                                        "{28A0092B-C50C-407E-A947-70E740481C1C}"
                                                 })
                                         )
                                         {
                                             Embed = relationshipId,
                                             CompressionState =
                                             A.BlipCompressionValues.Print
                                         },
                                         new A.Stretch(
                                             new A.FillRectangle())),
                                     new PIC.ShapeProperties(
                                         new A.Transform2D(
                                             new A.Offset() { X = 0L, Y = 0L },
                                             new A.Extents() { Cx = 990000L, Cy = 792000L }),
                                         new A.PresetGeometry(
                                             new A.AdjustValueList()
                                         )
                                         { Preset = A.ShapeTypeValues.Rectangle }))
                             )
                             { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" })
                     )
                     {
                         DistanceFromTop = (UInt32Value)0U,
                         DistanceFromBottom = (UInt32Value)0U,
                         DistanceFromLeft = (UInt32Value)0U,
                         DistanceFromRight = (UInt32Value)0U,
                         EditId = "50D07946"
                     });

            // Append the reference to body, the element should be in a Run.
            //tc1.Append(new Paragraph(new Run(element)));
            wordDoc.MainDocumentPart.Document.Body.AppendChild(new Paragraph(new Run(element)));
        }

        private void CheckDataFolder()
        {
            var filePath = Environment.CurrentDirectory;
            var DataFolder = "\\DocumentData\\";
            if (Directory.Exists($"{filePath}{DataFolder}") == false)
            {
                Directory.CreateDirectory($"{filePath}{DataFolder}");
            }
            dataFolder = $"{filePath}{DataFolder}";
        }

        private void CheckTemplateFolder()
        {
            var filePath = Environment.CurrentDirectory;
            var DataFolder = "\\WordTemplate\\";
            if (Directory.Exists($"{filePath}{DataFolder}") == false)
            {
                Directory.CreateDirectory($"{filePath}{DataFolder}");
            }
            templateFolder = $"{filePath}{DataFolder}";
            
        }

        public bool CheckTemplateFile()
        {
            
            if (File.Exists($"{dataFolder}Valvontamuistio.docx"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

