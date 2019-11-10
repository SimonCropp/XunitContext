 using System;
 using System.IO;
 using System.Reflection;
 using System.Threading;
 using ApprovalTests.Core;
 using ApprovalTests.Html;
 using ApprovalTests.Reporters;
 using ApprovalTests.Scrubber;
 using ApprovalTests.Utilities;
 using ApprovalTests.Writers;
 using ApprovalTests.Xml;
 using ApprovalUtilities.Utilities;

 namespace Xunit.ApprovalTests
 {
     public static class Approvals
     {

         static AsyncLocal<TestData?> testData = new AsyncLocal<TestData?>();

         public static void SetTestData(Type testType, MethodInfo testMethod)
         {
             testData.Value = new TestData(testType, testMethod);
         }

         public static void ClearTestData()
         {
             testData.Value = null;
         }

         public static void Verify(IApprovalWriter writer, IApprovalFailureReporter reporter)
         {
             var normalizeLineEndingsForTextFiles = GetTestData().GetFirstFrameForAttribute<IgnoreLineEndingsAttribute>();
             var shouldIgnoreLineEndings = normalizeLineEndingsForTextFiles == null || normalizeLineEndingsForTextFiles.IgnoreLineEndings;
             var approver = GetDefaultApprover(writer, shouldIgnoreLineEndings);
             Verify(approver, reporter);
         }

         static TestData GetTestData()
         {
             if (testData.Value == null) throw new Exception("SetTestData has not been called");
             return testData.Value;
         }

         static IApprovalApprover GetDefaultApprover(IApprovalWriter writer, bool shouldIgnoreLineEndings)
         {
             return global::ApprovalTests.Approvals.GetDefaultApprover(writer, Namer.Instance, shouldIgnoreLineEndings);
         }

         public static void Verify(IApprovalApprover approver)
         {
             Approver.Verify(approver, GetReporter());
         }

         public static void Verify(IApprovalApprover approver, IApprovalFailureReporter reporter)
         {
             Approver.Verify(approver, reporter);
         }

         public static IApprovalFailureReporter GetReporter()
         {
             return GetReporter(IntroductionReporter.INSTANCE);
         }

         public static IApprovalFailureReporter GetReporter(IApprovalFailureReporter defaultIfNotFound)
         {
             return GetFrontLoadedReporter(defaultIfNotFound, GetFrontLoadedReporterFromAttribute());
         }

         static IEnvironmentAwareReporter GetFrontLoadedReporterFromAttribute()
         {
             var frontLoaded = GetTestData().GetFirstFrameForAttribute<FrontLoadedReporterAttribute>();
             //TODO: DefaultFrontLoaderReporter => FrontLoadedReporterDisposer;
             return frontLoaded != null ? frontLoaded.Reporter : DefaultFrontLoaderReporter.INSTANCE;
         }

         static IApprovalFailureReporter GetFrontLoadedReporter(IApprovalFailureReporter defaultIfNotFound,
             IEnvironmentAwareReporter frontLoad)
         {
             return frontLoad.IsWorkingInThisEnvironment("default.txt")
                 ? frontLoad
                 : GetReporterFromAttribute() ?? defaultIfNotFound;
         }

         static IApprovalFailureReporter? GetReporterFromAttribute()
         {
             var useReporter = GetTestData().GetFirstFrameForAttribute<UseReporterAttribute>();
             return useReporter?.Reporter;
         }

         public static void Verify(IApprovalWriter writer)
         {
             Verify(writer, GetReporter());
         }

         public static void VerifyFile(string receivedFilePath)
         {
             Verify(new ExistingFileWriter(receivedFilePath));
         }

         public static void Verify(FileInfo receivedFilePath)
         {
             VerifyFile(receivedFilePath.FullName);
         }

         public static void Verify(object text)
         {
             Verify(WriterFactory.CreateTextWriter("" + text));
         }

         public static void Verify(string text, Func<string, string>? scrubber = null)
         {
             if (scrubber == null)
             {
                 scrubber = ScrubberUtils.NO_SCRUBBER;
             }

             Verify(WriterFactory.CreateTextWriter(scrubber(text)));
         }

         public static void Verify(Exception e)
         {
             Verify(e.Scrub());
         }

         public static void VerifyBinaryFile(byte[] bytes, string fileExtensionWithoutDot)
         {
             Verify(new ApprovalBinaryWriter(bytes, fileExtensionWithoutDot));
         }

         public static void VerifyHtml(string html)
         {
             HtmlApprovals.VerifyHtml(html);
         }

         public static void VerifyXml(string xml)
         {
             XmlApprovals.VerifyXml(xml);
         }

         public static void VerifyJson(string json)
         {
             Verify(WriterFactory.CreateTextWriter(json.FormatJson(), "json"));
         }

         public static void VerifyPdfFile(string pdfFilePath)
         {
             PdfScrubber.ScrubPdf(pdfFilePath);
             Verify(new ExistingFileWriter(pdfFilePath));
         }
     }
 }