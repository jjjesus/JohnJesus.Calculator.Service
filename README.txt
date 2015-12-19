

netsh http add urlacl url=http://+:8000/JohnJesus.Calculator.Service/service user=Everyone


svcutil /async /t:code /l:csharp /d:..\JohnJesus.Calculator.Client /n:*,JohnJesus.Calculator /out:CalculatorClient.cs /noconfig http://localhost:8
000/JohnJesus.Calculator.Service/service/mex