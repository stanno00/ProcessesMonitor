using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using NUnit.Framework;
using ProcessesMonitor;

namespace ProcessesMonitorTest;

public class Tests
{
    private StringBuilder _ConsoleOutput;
    private Mock<TextReader> _ConsoleInput;
    
    [SetUp]
    public void Setup()
    {
        _ConsoleOutput = new StringBuilder();
        var consoleOutputWriter = new StringWriter(_ConsoleOutput);
        _ConsoleInput = new Mock<TextReader>();
        Console.SetOut(consoleOutputWriter);
        Console.SetIn(_ConsoleInput.Object);
    }

    [Test]
    public void GetInput_IncorrectNumberOfParameters_ReturnsEmptyStringArray()
    {
        string[] input1 = {"notepad", "1"};
        string[] input2 = {"notepad", "1", "2", "3"};
        var expectedOutput = new string[]{};
        
        var actualOutput1 = ProcessesChecker.GetInput(input1);
        var actualOutput2 = ProcessesChecker.GetInput(input2);
        
        Assert.AreEqual(expectedOutput, actualOutput1);
        Assert.AreEqual(expectedOutput, actualOutput2);
    }

    [Test]
    public void GetInput_CorrectNumberOfParameters_ReturnStringArray()
    {
        string[] input1 = {"notepad", "1", "1"};
        string[] expectedOutput = {"notepad", "1", "1"};
        
        var actualOutput1 = ProcessesChecker.GetInput(input1);
        
        Assert.AreEqual(expectedOutput, actualOutput1);
    }

    [Test]
    public void CorrectInput_IncorrectParameters_ReturnEmptyStringArray()
    {
        string[] input1 = {"notepad", "a", "1"};
        string[] input2 = {"notepad", "a", "a"};
        string[] input3 = {"notepad", "1", "a"};
        string[] expectedOutput = new string[] {};

        var actualOutput1 = ProcessesChecker.CorrectInput(input1);
        var actualOutput2 = ProcessesChecker.CorrectInput(input2);
        var actualOutput3 = ProcessesChecker.CorrectInput(input3);

        Assert.AreEqual(expectedOutput, actualOutput1);
        Assert.AreEqual(expectedOutput, actualOutput2);
        Assert.AreEqual(expectedOutput, actualOutput3);
    }

    [Test]
    public void CorrectInput_CorrectParameters_ReturnTheSameStringArray()
    {
        string[] input1 = {"notepad", "1", "1"};
        
        var actualOutput1 = ProcessesChecker.CorrectInput(input1);
        
        Assert.AreEqual(input1, actualOutput1);
    }

    [Test]
    public void CheckForProcesses_WhenProcessesExist_WillCloseIt()
    {
        Process.Start(@"notepad.exe");
        Process[] expectedOutput = { };
        
        var actualOutput1 = ProcessesChecker.CheckForProcesses("notepad", 0);
        
        Assert.AreEqual(expectedOutput, Process.GetProcessesByName("notepad"));
        Assert.AreEqual("Processes notepad was ended at " + DateTime.Now, actualOutput1);
    }
    
    [Test]
    public void CheckForProcesses_WhenProcessesDoesntExist_ReturnEmptyString()
    {
        string expectedOutput = "";
        
        var actualOutput1 = ProcessesChecker.CheckForProcesses("?mvcn", 0);
        
        Assert.AreEqual(expectedOutput, actualOutput1);
    }
}