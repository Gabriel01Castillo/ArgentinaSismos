Public MustInherit Class Geometry(Of T)
    Inherits GeoObject

    Protected Sub New(ByVal t As ObjectType)
        MyBase.New(t)
    End Sub

    Private _Coordinates As T
    <Newtonsoft.Json.JsonProperty("coordinates")> _
    Public Property Coordinates() As T
        Get
            Return _Coordinates
        End Get
        Set(ByVal value As T)
            _Coordinates = value
        End Set
    End Property
End Class