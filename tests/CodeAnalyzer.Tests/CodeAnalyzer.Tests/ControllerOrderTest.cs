using FluentAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeAnalyzer.Tests;

public class ControllerOrderTest
{
    private string[] _validOrder;

    [SetUp]
    public void Setup()
    {
        this._validOrder = new[] { "Get", "Post", "Put", "Patch", "Delete", "Head", "Options" };
    }

    [Test]
    public void Controller_Methods_Have_Valid_Order()
    {
        // Arrange
        string projectDirectory = "../../../../../../";

        // Act
        string[] controllerFiles = Directory.GetFiles(
            projectDirectory,
            @"*Controller.cs",
            SearchOption.AllDirectories);

        foreach (var filePath in controllerFiles)
        {
            if (filePath.Contains("ApplicationController"))
            {
                continue;
            }

            // Чтение кода из файла
            string code = File.ReadAllText(filePath);

            // Разбор синтаксиса
            SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
            CompilationUnitSyntax root = tree.GetCompilationUnitRoot();

            // Поиск классов, унаследованных от Controller
            var controllerClasses = root.DescendantNodes()
                .OfType<ClassDeclarationSyntax>()
                .Where(c => c.BaseList != null &&
                            c.BaseList.Types.Any(t => t.ToString().Contains("Controller")));

            List<ControllerPayload> list;

            foreach (var controller in controllerClasses)
            {
                list = new List<ControllerPayload>();

                // Поиск методов с HTTP-атрибутами
                var methodsWithHttpAttributes = controller.Members
                    .OfType<MethodDeclarationSyntax>()
                    .Where(m => m.AttributeLists.SelectMany(a => a.Attributes)
                        .Any(attr => attr.Name.ToString().StartsWith("Http")));

                foreach (var method in methodsWithHttpAttributes)
                {
                    // Список HTTP-атрибутов
                    var httpAttribute = method.AttributeLists
                        .SelectMany(a => a.Attributes)
                        .Where(attr => attr.Name.ToString().StartsWith("Http"))
                        .Select(attr => attr.Name.ToString().Substring(4)).ToList();

                    httpAttribute.Count.Should().Be(1);

                    list.Add(new ControllerPayload(method.Identifier.Text, httpAttribute[0]));
                }

                // Assert
                this.CheckForValidOrder(list, controller.Identifier.Text, filePath.Substring(18));
            }
        }
    }

    private void CheckForValidOrder(List<ControllerPayload> input, string controllerName, string path)
    {
        // Переменная, отслеживающая текущую позицию в эталонном порядке
        int currentIndex = -1;

        foreach (var operation in input)
        {
            // Ищем индекс текущей операции в эталонном порядке
            int operationIndex = Array.IndexOf(this._validOrder, operation.AttributeName);

            if (operationIndex == -1)
            {
                // Если операция не найдена в эталонном порядке
                throw new Exception(controllerName + ": Видимо у тебя есть аттрибут начинающий на Http " +
                                                  "но при этом не рест глагол, уточни вопрос с кем-нибудь на счет ситуации");
            }

            if (operationIndex < currentIndex)
            {
                // Если порядок нарушен, бросаем ошибку
                throw new Exception("Path: " + path + "\nController: " + controllerName + "\nMethod: " + operation.MethodName + "\nПрошу придерживаться валидного порядка методов контроллера чтобы всем было легче жить " +
                                                  "\"Get\", \"Post\", \"Put\", \"Patch\", \"Delete\", \"Head\", \"Options\"");
            }

            // Обновляем текущий индекс
            currentIndex = operationIndex;
        }
    }

    private record ControllerPayload(string MethodName, string AttributeName);
}
