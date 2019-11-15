 using System;
 using System.IO;
 using System.Reflection;
 using System.Threading;
 using ApprovalTests.Core;
 using ApprovalTests.Html;
 using ApprovalTests.Reporters;
 using ApprovalTests.Scrubber;
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

         public static void Verify(IApprovalWriter writer, IApprovalFailureReporter reporter, bool ignoreLineEndings = false)
         {
             var approver = GetDefaultApprover(writer, ignoreLineEndings);
             Verify(approver, reporter);
         }

         static TestData GetTestData()
         {
             if (testData.Value == null)
             {
                 throw new Exception("SetTestData has not been called");
             }

             return testData.Value;
         }

         static IApprovalApprover GetDefaultApprover(IApprovalWriter writer, bool ignoreLineEndings = false)
         {
             return global::ApprovalTests.Approvals.GetDefaultApprover(writer, Namer.Instance, ignoreLineEndings);
         }

         public static void Verify(IApprovalApprover approver)
         {
             Approver.Verify(approver, GetReporter());
         }

         public static void Verify(IApprovalApprover approver, IApprovalFailureReporter reporter)
         {
             Approver.Verify(approver, reporter);
         }

         static IApprovalFailureReporter GetReporter()
         {
             return GetReporter(IntroductionReporter.INSTANCE);
         }

         static IApprovalFailureReporter GetReporter(IApprovalFailureReporter defaultIfNotFound)
         {
             var data = GetTestData();
             var loadedReporterFromAttribute = GetFrontLoadedReporterFromAttribute(data);
             return GetFrontLoadedReporter(defaultIfNotFound, loadedReporterFromAttribute, data);
         }

         static IEnvironmentAwareReporter GetFrontLoadedReporterFromAttribute(TestData data)
         {
             var frontLoaded = data.GetFirstFrameForAttribute<FrontLoadedReporterAttribute>();
             //TODO: DefaultFrontLoaderReporter => FrontLoadedReporterDisposer;
             if (frontLoaded == null)
             {
                 return DefaultFrontLoaderReporter.INSTANCE;
             }

             return frontLoaded.Reporter;
         }

         static IApprovalFailureReporter GetFrontLoadedReporter(
             IApprovalFailureReporter defaultIfNotFound,
             IEnvironmentAwareReporter frontLoad,
             TestData data)
         {
             if (frontLoad.IsWorkingInThisEnvironment("default.txt"))
             {
                 return frontLoad;
             }

             return GetReporterFromAttribute(data) ?? defaultIfNotFound;
         }

         static IApprovalFailureReporter? GetReporterFromAttribute(TestData data)
         {
             var useReporter = data.GetFirstFrameForAttribute<UseReporterAttribute>();
             return useReporter?.Reporter;
         }

         public static void Verify(IApprovalWriter writer, bool ignoreLineEndings)
         {
             Verify(writer, GetReporter(), ignoreLineEndings);
         }

         public static void VerifyFile(string receivedFilePath, bool ignoreLineEndings = false)
         {
             Verify(new ExistingFileWriter(receivedFilePath), ignoreLineEndings);
         }

         public static void Verify(FileInfo receivedFilePath, bool ignoreLineEndings = false)
         {
             VerifyFile(receivedFilePath.FullName, ignoreLineEndings);
         }

         public static void Verify(object text, bool ignoreLineEndings)
         {
             Verify(WriterFactory.CreateTextWriter("" + text), ignoreLineEndings);
         }

         public static void Verify(string text, Func<string, string>? scrubber = null, bool ignoreLineEndings = false)
         {
             if (scrubber == null)
             {
                 scrubber = ScrubberUtils.NO_SCRUBBER;
             }

             Verify(WriterFactory.CreateTextWriter(scrubber(text)), ignoreLineEndings);
         }

         public static void VerifyBinaryFile(byte[] bytes, string fileExtensionWithoutDot)
         {
             Verify(new ApprovalBinaryWriter(bytes, fileExtensionWithoutDot), false);
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
             Verify(WriterFactory.CreateTextWriter(json.FormatJson(), "json"), false);
         }

         public static void VerifyPdfFile(string pdfFilePath)
         {
             PdfScrubber.ScrubPdf(pdfFilePath);
             Verify(new ExistingFileWriter(pdfFilePath), false);
         }
     }
 }