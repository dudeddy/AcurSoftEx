Imports System.ComponentModel
Imports AcurSoft.XtraEditors.Base
Imports AcurSoft.XtraEditors.ViewInfo
Imports DevExpress.Utils
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraEditors.Drawing
Imports DevExpress.XtraEditors.Mask
Imports DevExpress.XtraEditors.Registrator
Imports DevExpress.XtraEditors.Repository


Namespace AcurSoft.XtraEditors.Repository
    <UserRepositoryItem("RegisterQuarterEdit")>
    Public Class RepositoryItemQuarterEdit
        Inherits RepositoryItemLookUpEdit
        Implements ISpinButtonEditor
        'The unique name for the custom editor
        Public Const CustomEditName As String = "QuarterEdit"

        Private Shared _Quarters As Dictionary(Of Integer, String)
        Public Shared ReadOnly Property Quarters As Dictionary(Of Integer, String)
            Get
                If _Quarters Is Nothing Then
                    _Quarters = New Dictionary(Of Integer, String)
                    For i As Integer = 1 To 4
                        _Quarters.Add(i, "Q" & i)
                    Next
                End If
                Return _Quarters
            End Get
        End Property



        Shared Sub New()
            RegisterEdit()
        End Sub

        Private _SpinButtonIndex As Integer = 0
        Public Property SpinButtonIndex() As Integer Implements ISpinButtonEditor.SpinButtonIndex
            Get
                Return _SpinButtonIndex
            End Get
            Set(ByVal value As Integer)
                _SpinButtonIndex = value
            End Set
        End Property


        Public Sub New()
            MyBase.New()
            'Me.DisplayFormat.FormatString = "yyyy"
            'Me.EditFormat.FormatType = FormatType.Numeric
            'Me.EditFormat.FormatString = "N00"
            Me.DataSource = RepositoryItemQuarterEdit.Quarters
            Me.TextEditStyle = TextEditStyles.Standard
            Me.ShowHeader = False
            Me.ShowFooter = False
            Me.ActionButtonIndex = 1
        End Sub

        Protected Overrides Function CreateEditFormat() As FormatInfo
            Dim formatInfo As New FormatInfo()
            formatInfo.FormatType = FormatType.Numeric
            formatInfo.FormatString = "N00"
            Return formatInfo
            'Return MyBase.CreateEditFormat()
        End Function

        Public Overrides ReadOnly Property Mask As MaskProperties
            Get
                Dim m As New MaskProperties
                m.MaskType = MaskType.Numeric
                m.EditMask = "N00"
                'm.UseMaskAsDisplayFormat = True
                Return m
                'Return MyBase.Mask
            End Get
        End Property

        'Return the unique name
        Public Overrides ReadOnly Property EditorTypeName() As String
            Get
                Return CustomEditName
            End Get
        End Property


        Public ReadOnly Property SpinButton As EditorButton
        'Public ReadOnly Property SpinButtonDown As EditorButton
        Protected Overrides Function CreateButtonCollection() As EditorButtonCollection
            Dim col As EditorButtonCollection = MyBase.CreateButtonCollection()
            _SpinButton = New EditorButton(ButtonPredefines.SpinUp) With {.Tag = "SPIN"}
            col.Add(_SpinButton)
            Return col
        End Function

        Protected Overrides Sub RaiseEditValueChangedCore(e As EventArgs)
            'Dim value As Integer = Convert.ToInt32(Me.QuarterEdit.EditValue)
            Dim quarter As Integer = Convert.ToInt32(Me.QuarterEdit.EditValue)
            If quarter < 1 OrElse quarter > 4 Then
                Return
            End If

            MyBase.RaiseEditValueChangedCore(e)

        End Sub

        Protected Overrides Sub RaiseEditValueChanging(e As ChangingEventArgs)
            If e.NewValue Is Nothing Then
                e.Cancel = True
            Else
                Try
                    Dim quarter As Integer = Convert.ToInt32(e.NewValue)
                    If quarter < 1 OrElse quarter > 4 Then
                        e.Cancel = True
                    End If
                Catch ex As Exception
                    e.Cancel = True
                End Try
            End If
            MyBase.RaiseEditValueChanging(e)
        End Sub


        Protected Overrides Sub RaiseButtonPressed(e As ButtonPressedEventArgs)
            If e.Button.Kind = ButtonPredefines.SpinUp OrElse e.Button.Kind = ButtonPredefines.SpinDown Then
                Dim quarter As Integer = Convert.ToInt32(Me.QuarterEdit.EditValue)
                If e.Button.Kind = ButtonPredefines.SpinUp Then
                    quarter += 1
                Else
                    quarter -= 1
                End If
                If quarter = 5 Then
                    quarter = 1
                ElseIf quarter = 0
                    quarter = 4
                End If
                Me.QuarterEdit.EditValue = quarter
            End If
            MyBase.RaiseButtonPressed(e)

        End Sub


        Public ReadOnly Property QuarterEdit As QuarterEdit
            Get
                Return DirectCast(Me.OwnerEdit, QuarterEdit)
            End Get
        End Property


        'Register the editor
        Public Shared Sub RegisterEdit()
            'Icon representing the editor within a container editor's Designer
            Dim img As Image = Nothing
            Try
                img = CType(Bitmap.FromStream(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("DevExpress.CustomEditors.CustomEdit.bmp")), Bitmap)
            Catch
            End Try
            'EditorRegistrationInfo.Default.Editors.Add(New EditorClassInfo(CustomEditName, GetType(YearEdit), GetType(RepositoryItemYearEdit), GetType(DateEditViewInfo), New ButtonEditPainter(), True, img))
            EditorRegistrationInfo.Default.Editors.Add(New EditorClassInfo(CustomEditName, GetType(QuarterEdit), GetType(RepositoryItemQuarterEdit), GetType(SpinButtonEditExViewInfo), New ButtonEditPainter, True, img))
        End Sub

        'Override the Assign method
        Public Overrides Sub Assign(ByVal item As RepositoryItem)
            BeginUpdate()
            Try
                MyBase.Assign(item)
                Dim source As RepositoryItemQuarterEdit = TryCast(item, RepositoryItemQuarterEdit)
                If source Is Nothing Then
                    Return
                Else
                    source.SpinButtonIndex = Me.SpinButtonIndex
                End If
            Finally
                EndUpdate()
            End Try
        End Sub

        Protected Overrides ReadOnly Property AllowFormatEditValue As Boolean
            Get
                Return False
            End Get
        End Property

        Protected Overrides ReadOnly Property AllowParseEditValue As Boolean
            Get
                Return True
            End Get
        End Property


    End Class
End Namespace

