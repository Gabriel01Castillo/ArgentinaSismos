Public Class Point
    Inherits Geometry(Of Double())

    Public Sub New()
        MyBase.New(ObjectType.Point)
        Coordinates = New Double() {0, 0}
    End Sub
    Public Sub New(ByVal Lat As Double, ByVal Lon As Double)
        MyClass.New()

        Me.Lat = Lat
        Me.Lon = Lon
    End Sub
    Public Sub New(ByVal S As String)
        MyClass.New()

        With S.Split(",")
            Lat = Double.Parse(.GetValue(0))
            Lon = Double.Parse(.GetValue(1))
        End With
    End Sub

    <Newtonsoft.Json.JsonIgnore()> Public Property Lat() As Double
        Get
            Return Coordinates(1)
        End Get
        Set(ByVal value As Double)
            Coordinates(1) = value
        End Set
    End Property
    <Newtonsoft.Json.JsonIgnore()> Public Property Lon() As Double
        Get
            Return Coordinates(0)
        End Get
        Set(ByVal value As Double)
            Coordinates(0) = value
        End Set
    End Property

    Public Overrides Function ToString() As String
        Return "{0},{1}".Compose(Lat, Lon)
    End Function

    Shared Narrowing Operator CType(ByVal Original As Point) As String
        Return Original.ToString
    End Operator
    Shared Widening Operator CType(ByVal Original As String) As Point
        Return New Point(Original)
    End Operator
End Class