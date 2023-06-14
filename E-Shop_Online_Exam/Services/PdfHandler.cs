using EShopOnlineExam.Repository;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using EShopOnlineExam.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Routing;
using MessagePack.Resolvers;

namespace EShopOnlineExam.Services
{
    public class PdfHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        public PdfHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork= unitOfWork;
        }
        MemoryStream ms = new MemoryStream();
        public async Task<byte[]> GenerateCertificakePdf(int id)
        {

            Document document = new Document(PageSize.A4, 25, 25, 30, 30);
            PdfWriter writer = PdfWriter.GetInstance(document, ms);
            document.Open();


            //var path = Path.Combine(Directory.GetCurrentDirectory(), "Image");
            //var  PhysicalFile(path, "image/jpeg");
            string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "Image" , "Image.png");//.Current.Server.MapPath("~/Image/Image.png");


            AddImage(imagePath, document);

            //GenerateText(document, "Can you beleve it i even manage to add an image to PDF ! ! !\n");

            //GenerateText(document, "What next ? \n");
            //GenerateText(document, "A TABLE ! ! !\n\n\n");


            List<string> cellHeaders = new List<string>();
            List<PdfPCell> ListOfCellHeaders = new List<PdfPCell>();
            ListOfCellHeaders.Add(new PdfPCell(new Phrase("Topic Title", new Font(Font.FontFamily.HELVETICA, 10))));
            ListOfCellHeaders.Add(new PdfPCell(new Phrase("Maximun Marks", new Font(Font.FontFamily.HELVETICA, 10))));
            ListOfCellHeaders.Add(new PdfPCell(new Phrase("Awarded Marks", new Font(Font.FontFamily.HELVETICA, 10))));
            ListOfCellHeaders.Add(new PdfPCell(new Phrase("Success Rate", new Font(Font.FontFamily.HELVETICA, 10))));

            var table = GenerateTable(ListOfCellHeaders);
            var constant = await ImportDataToTable(await _unitOfWork.CandidateExamination.GetWithExam(id), table, document, writer);

            return constant;
        }

        private void GenereteHeaders(Document document, string HeaderText)
        {
            Paragraph paragraph = new Paragraph($"{HeaderText}", new Font(Font.FontFamily.HELVETICA, 20));
            paragraph.Alignment = Element.ALIGN_CENTER;
            document.Add(paragraph);
        }
        private void GenerateText(Document document, string Text)
        {
            Paragraph paragraph = new Paragraph($"{Text}", new Font(Font.FontFamily.HELVETICA, 12));
            paragraph.Alignment = Element.ALIGN_CENTER;
            document.Add(paragraph);
        }
        private void AddImage(string imagePath, Document document)
        {
            var image = iTextSharp.text.Image.GetInstance(imagePath);
            image.Alignment = Element.ALIGN_CENTER;
            document.Add(image);

        }
        private PdfPTable GenerateTable(List<PdfPCell> ListOfCellHeaders)
        {
            PdfPTable table = new PdfPTable(4);
            foreach (var cell in ListOfCellHeaders)
            {

                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                cell.BorderWidthBottom = 1f;
                cell.BorderWidthTop = 1f;
                cell.BorderWidthLeft = 1f;
                cell.BorderWidthRight = 1f;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.VerticalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
            }
            return table;
        }

        private async Task<byte[]> ImportDataToTable(CandidateExamination exam, PdfPTable table, Document document, PdfWriter writer)
        {
            var candidateResults = (List<CandidateResults>)await _unitOfWork.CandidateResults.WhereExaminationIdFull(exam.Id);
            var examTopics = (List < ExamTopics >) await _unitOfWork.ExamTopic.GetEnableExamTopic(exam.Exam.Id);
            //var topicScores = (List<CandidateResults>)await _unitOfWork.CandidateResults.WhereExaminationId(exam.Id);
            for (int i = 0; i < examTopics.Count(); i++)
            {
                PdfPCell cell_1 = new PdfPCell(new Phrase(examTopics[i].Topic.Title.ToString()));
                PdfPCell cell_2 = new PdfPCell(new Phrase(examTopics[i].SubjectWeight.ToString()));
                int score = 0;
                foreach (var candidateResult in candidateResults)
                {
                    if (examTopics[i].Id == candidateResult.ExamQuestion.ExamTopics.Id)
                    {
                        if(candidateResult.QuestionResult > 0)
                        {
                            score++;
                        }
                    }
                }

                PdfPCell cell_3 = new PdfPCell(new Phrase(score.ToString()));
                double x = score;
                double y = examTopics[i].SubjectWeight;
                var m = Math.Round((x / y * 100), 2, MidpointRounding.ToEven).ToString();
                m += " %";

                PdfPCell cell_4 = new PdfPCell(new Phrase(m));

                cell_1.HorizontalAlignment = Element.ALIGN_CENTER;
                cell_2.HorizontalAlignment = Element.ALIGN_CENTER;
                cell_3.HorizontalAlignment = Element.ALIGN_CENTER;
                cell_4.HorizontalAlignment = Element.ALIGN_CENTER;
                if (i % 2 != 0)
                {
                    cell_1.BackgroundColor = BaseColor.LIGHT_GRAY;
                    cell_2.BackgroundColor = BaseColor.LIGHT_GRAY;
                    cell_3.BackgroundColor = BaseColor.LIGHT_GRAY;
                    cell_4.BackgroundColor = BaseColor.LIGHT_GRAY;
                }
                table.AddCell(cell_1);
                table.AddCell(cell_2);
                table.AddCell(cell_3);
                table.AddCell(cell_4);
            }
            document.Add(table);
            document.Close();
            writer.Close();
            return ms.ToArray();
        }
    }
}
