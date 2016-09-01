Public Module Extensions
    <Runtime.CompilerServices.Extension()> _
    Public Function Compose(ByVal format As String, ByVal ParamArray args As Object()) As String
        Return String.Format( _
            Globalization.CultureInfo.InvariantCulture, _
            format, _
 _
            Aggregate arg In args _
            Let a = If(arg, String.Empty) _
            Select If( _
                TypeOf a Is IFormattable, _
                DirectCast(a, IFormattable).ToString( _
                    String.Empty, _
                    Globalization.CultureInfo.InvariantCulture), _
                a.ToString) _
            Into ToArray())

    End Function
End Module
