Namespace AcurSoft.Data


    Public Class TimeSpanFormatHelper
        Implements IFormatProvider, ICustomFormatter
        Public Const DEFAULT_DISPLAY_FORMAT As String = "{0:d\.h\:mm\:ss}"
        Public Sub New()
            Me.New(DEFAULT_DISPLAY_FORMAT)
        End Sub
        Public Sub New(displayFormat As String)
            If String.IsNullOrEmpty(displayFormat) Then
                displayFormat = DEFAULT_DISPLAY_FORMAT
            End If
            Me.DisplayFormat = displayFormat
        End Sub

        Private Function GetFormat(ByVal formatType As Type) As Object Implements IFormatProvider.GetFormat
            Return Me
        End Function

        Private Function ICustomFormatter_Format(ByVal format As String, ByVal arg As Object, ByVal formatProvider As IFormatProvider) As String Implements ICustomFormatter.Format
            Return CType(arg, TimeSpan).ToString(format)
        End Function

        Public Property DisplayFormat As String
    End Class
End Namespace
