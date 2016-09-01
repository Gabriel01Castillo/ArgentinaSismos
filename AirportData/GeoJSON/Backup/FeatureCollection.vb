Public Class FeatureCollection(Of T)
    Inherits GeoObject

    Public Sub New()
        MyBase.New(ObjectType.FeatureCollection)
    End Sub

    Private _Features As T()
    <Newtonsoft.Json.JsonProperty("features")> _
    Public Property Features() As T()
        Get
            Return _Features
        End Get
        Set(ByVal value As T())
            _Features = value
        End Set
    End Property
End Class