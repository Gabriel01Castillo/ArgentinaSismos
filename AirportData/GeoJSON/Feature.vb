Public Class Feature(Of TGeometry, TProperties)
    Inherits GeoObject

    Public Sub New()
        MyBase.New(ObjectType.Feature)
    End Sub

    Private _Geometry As TGeometry
    <Newtonsoft.Json.JsonProperty("geometry")> _
    Public Property Geometry() As TGeometry
        Get
            Return _Geometry
        End Get
        Set(ByVal value As TGeometry)
            _Geometry = value
        End Set
    End Property

    Private _Properties As TProperties
    <Newtonsoft.Json.JsonProperty("properties")> _
    Public Property Properties() As TProperties
        Get
            Return _Properties
        End Get
        Set(ByVal value As TProperties)
            _Properties = value
        End Set
    End Property
End Class