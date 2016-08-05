Imports DevExpress.XtraEditors

Public Class XRangeControlClient
    Implements IRangeControlClient

    Public ReadOnly Property InvalidText As String Implements IRangeControlClient.InvalidText
        Get
            Return ""
        End Get
    End Property

    Public ReadOnly Property IsCustomRuler As Boolean Implements IRangeControlClient.IsCustomRuler
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property IsValid As Boolean Implements IRangeControlClient.IsValid
        Get
            Return True
        End Get
    End Property

    Public ReadOnly Property NormalizedRulerDelta As Double Implements IRangeControlClient.NormalizedRulerDelta
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property RangeBoxBottomIndent As Integer Implements IRangeControlClient.RangeBoxBottomIndent
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property RangeBoxTopIndent As Integer Implements IRangeControlClient.RangeBoxTopIndent
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property RulerDelta As Object Implements IRangeControlClient.RulerDelta
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Public Event RangeChanged As ClientRangeChangedEventHandler Implements IRangeControlClient.RangeChanged

    Public Sub Calculate(contentRect As Rectangle) Implements IRangeControlClient.Calculate
        Throw New NotImplementedException()
    End Sub

    Public Sub DrawContent(e As RangeControlPaintEventArgs) Implements IRangeControlClient.DrawContent
        Throw New NotImplementedException()
    End Sub

    Public Sub OnClick(hitInfo As RangeControlHitInfo) Implements IRangeControlClient.OnClick
        'Throw New NotImplementedException()
    End Sub

    Public Sub OnRangeChanged(rangeMinimum As Object, rangeMaximum As Object) Implements IRangeControlClient.OnRangeChanged
        'Throw New NotImplementedException()
    End Sub

    Public Sub OnRangeControlChanged(rangeControl As IRangeControl) Implements IRangeControlClient.OnRangeControlChanged
        'Throw New NotImplementedException()
    End Sub

    Public Sub OnResize() Implements IRangeControlClient.OnResize
        'Throw New NotImplementedException()
    End Sub

    Public Sub UpdateHotInfo(hitInfo As RangeControlHitInfo) Implements IRangeControlClient.UpdateHotInfo
        Throw New NotImplementedException()
    End Sub

    Public Sub UpdatePressedInfo(hitInfo As RangeControlHitInfo) Implements IRangeControlClient.UpdatePressedInfo
        Throw New NotImplementedException()
    End Sub

    Public Sub ValidateRange(info As NormalizedRangeInfo) Implements IRangeControlClient.ValidateRange
        Throw New NotImplementedException()
    End Sub

    Public Function DrawRuler(e As RangeControlPaintEventArgs) As Boolean Implements IRangeControlClient.DrawRuler
        Throw New NotImplementedException()
    End Function

    Public Function GetNormalizedValue(value As Object) As Double Implements IRangeControlClient.GetNormalizedValue
        Throw New NotImplementedException()
    End Function

    Public Function GetOptions() As Object Implements IRangeControlClient.GetOptions
        'Throw New NotImplementedException()
        Return Me
    End Function

    Public Function GetRuler(e As RulerInfoArgs) As List(Of Object) Implements IRangeControlClient.GetRuler
        Throw New NotImplementedException()
    End Function

    Public Function GetValue(normalizedValue As Double) As Object Implements IRangeControlClient.GetValue
        Throw New NotImplementedException()
    End Function

    Public Function IsValidType(type As Type) As Boolean Implements IRangeControlClient.IsValidType
        Throw New NotImplementedException()
    End Function

    Public Function RulerToString(ruleIndex As Integer) As String Implements IRangeControlClient.RulerToString
        Throw New NotImplementedException()
    End Function

    Public Function SupportOrientation(orientation As Orientation) As Boolean Implements IRangeControlClient.SupportOrientation
        Throw New NotImplementedException()
    End Function

    Public Function ValidateScale(newScale As Double) As Double Implements IRangeControlClient.ValidateScale
        Throw New NotImplementedException()
    End Function

    Public Function ValueToString(normalizedValue As Double) As String Implements IRangeControlClient.ValueToString
        Throw New NotImplementedException()
    End Function
End Class
