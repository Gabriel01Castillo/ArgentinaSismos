Public MustInherit Class GeoObject
    Private _Type As ObjectType

    <Newtonsoft.Json.JsonConverter(GetType(Newtonsoft.Json.Converters.StringEnumConverter))> _
    <Newtonsoft.Json.JsonProperty("type")> _
    Public Overridable Property Type() As ObjectType
        Get
            Return _Type
        End Get
        Set(ByVal value As ObjectType)
            _Type = value
        End Set
    End Property

    Protected Sub New(ByVal type As ObjectType)
        _Type = type
    End Sub
End Class