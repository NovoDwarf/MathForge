# MathForge.Analyzers

`MFA001` предлагает заменить простые `if (...) throw new ArgumentOutOfRangeException(...)`
на методы `MathForge.Numerics.Exceptions.MathArgumentException`.

Поддерживаются `IsNaN`, `IsInfinity`, отрицание `IsFinite`, сравнения с нулём
и проверки `<`, `<=`, `>`, `>=` относительно границы. Анализ выполняется
по символам Roslyn для встроенных числовых типов.

```csharp
if (value <= 0)
{
    throw new ArgumentOutOfRangeException(nameof(value), value, "Value must be positive.");
}

// MFA001 → Use MathArgumentException
MathArgumentException.ThrowIfNegativeOrZero(value, nameof(value), "Value must be positive.");
```

Исправления предоставляет отдельный проект `MathForge.Analyzers.CodeFixes`;
доступен Fix All. Оба проекта подключены к проектам решения через
`Directory.Build.props` как анализаторы, без зависимости runtime-библиотек от Roslyn.

Предложение появляется только при сохранении имени параметра, сообщения и
`ActualValue`: для `IsNaN` нужен конструктор `(paramName, message)`, для остальных
проверок — `(paramName, value, message)`. Имя и сообщение должны быть строковыми
константами, проверяемое значение — локальной переменной, параметром или константой.
Произвольные вычисления, свойства, дополнительные действия, `else`, внутренние
комментарии, директивы и перегруженные операторы пропускаются. Одноаргументный
конструктор исключения, составные условия и `Range.Contains` пока не преобразуются.
Существующие проверки автоматически не переписываются.

Тесты анализатора проверяют диагностику, применение исправлений, компиляцию
полученного кода и отсутствие предложений для неподдерживаемых конструкций.
