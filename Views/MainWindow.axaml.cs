using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace GUI_introduction_excercises.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    // Ejercicio 1: Hola Mundo Mejorado
    private void HelloButton_Click(object? sender, RoutedEventArgs e)
    {
        if (HelloLabel != null && HelloButton != null)
        {
            HelloLabel.Text = "¡Hola, Avalonia!";
            HelloButton.IsEnabled = false;
        }
    }

    // Ejercicio 2: Conversor de Temperatura
    private void ConvertButton_Click(object? sender, RoutedEventArgs e)
    {
        if (TemperatureInput == null || ConversionType == null || ConversionResult == null || ConversionError == null)
            return;

        // Limpiar errores previos
        ConversionError.Text = "";

        // Validar entrada
        if (string.IsNullOrWhiteSpace(TemperatureInput.Text))
        {
            ConversionError.Text = "Por favor, ingrese una temperatura.";
            return;
        }

        if (!double.TryParse(TemperatureInput.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out double temperature))
        {
            ConversionError.Text = "Por favor, ingrese un número válido.";
            return;
        }

        // Realizar conversión
        double result;
        string unit;

        if (ConversionType.SelectedIndex == 0) // Celsius a Fahrenheit
        {
            result = (temperature * 9.0 / 5.0) + 32;
            unit = "°F";
        }
        else // Fahrenheit a Celsius
        {
            result = (temperature - 32) * 5.0 / 9.0;
            unit = "°C";
        }

        ConversionResult.Text = $"Resultado: {result:F2} {unit}";
    }

    // Ejercicio 3: Calculadora - Validación de entrada numérica
    private void NumberInput_TextChanged(object? sender, TextChangedEventArgs e)
    {
        if (sender is TextBox textBox && !string.IsNullOrEmpty(textBox.Text))
        {
            // Permitir solo números, punto decimal y signo negativo al inicio
            var regex = new Regex(@"^-?\d*\.?\d*$");
            if (!regex.IsMatch(textBox.Text))
            {
                // Si no es válido, remover el último carácter
                var currentPosition = textBox.CaretIndex;
                textBox.Text = textBox.Text.Substring(0, Math.Max(0, textBox.Text.Length - 1));
                textBox.CaretIndex = Math.Max(0, currentPosition - 1);
            }
        }
    }

    // Ejercicio 3: Operaciones de la calculadora
    private void AddButton_Click(object? sender, RoutedEventArgs e) => PerformCalculation('+');
    private void SubtractButton_Click(object? sender, RoutedEventArgs e) => PerformCalculation('-');
    private void MultiplyButton_Click(object? sender, RoutedEventArgs e) => PerformCalculation('*');
    private void DivideButton_Click(object? sender, RoutedEventArgs e) => PerformCalculation('/');

    private void PerformCalculation(char operation)
    {
        if (FirstNumber == null || SecondNumber == null || CalculatorResult == null || CalculatorError == null)
            return;

        // Limpiar errores previos
        CalculatorError.Text = "";

        // Validar entradas
        if (string.IsNullOrWhiteSpace(FirstNumber.Text) || string.IsNullOrWhiteSpace(SecondNumber.Text))
        {
            CalculatorError.Text = "Por favor, ingrese ambos números.";
            return;
        }

        if (!double.TryParse(FirstNumber.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out double num1) ||
            !double.TryParse(SecondNumber.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out double num2))
        {
            CalculatorError.Text = "Por favor, ingrese números válidos.";
            return;
        }

        // Realizar operación
        double result;
        try
        {
            switch (operation)
            {
                case '+':
                    result = num1 + num2;
                    break;
                case '-':
                    result = num1 - num2;
                    break;
                case '*':
                    result = num1 * num2;
                    break;
                case '/':
                    if (num2 == 0)
                    {
                        CalculatorError.Text = "Error: No se puede dividir entre cero.";
                        return;
                    }
                    result = num1 / num2;
                    break;
                default:
                    CalculatorError.Text = "Operación no válida.";
                    return;
            }

            CalculatorResult.Text = $"Resultado: {result:F6}".TrimEnd('0').TrimEnd('.');
        }
        catch (Exception ex)
        {
            CalculatorError.Text = $"Error en el cálculo: {ex.Message}";
        }
    }
}