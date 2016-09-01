Public Class GeometryCollection(Of T)
    Inherits GeoObject

    Public Sub New()
        MyBase.New(ObjectType.GeometryCollection)
    End Sub

    Private _Geometries As T()
    <Newtonsoft.Json.JsonProperty("geometries")> _
    Public Property Geometries() As T()
        Get
            Return _Geometries
        End Get
        Set(ByVal value As T())
            _Geometries = value
        End Set
    End Property
End Class