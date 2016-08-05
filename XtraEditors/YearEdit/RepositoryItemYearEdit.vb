Imports System.ComponentModel
Imports AcurSoft.XtraEditors.Base
Imports AcurSoft.XtraEditors.ViewInfo
Imports DevExpress.Utils
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraEditors.Drawing
Imports DevExpress.XtraEditors.Mask
Imports DevExpress.XtraEditors.Registrator
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraEditors.ViewInfo

Namespace AcurSoft.XtraEditors.Repository
    <UserRepositoryItem("RegisterYearEdit")>
    Public Class RepositoryItemYearEdit
        Inherits RepositoryItemDateEdit
        Implements ISpinButtonEditor
        'The unique name for the custom editor
        Public Const CustomEditName As String = "YearEdit"

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

        'Protected Overrides Sub RaiseSpin(e As SpinEventArgs)
        '    MyBase.RaiseSpin(e)
        'End Sub

        Public Sub New()
            MyBase.New()
            Me.VistaCalendarInitialViewStyle = DevExpress.XtraEditors.VistaCalendarInitialViewStyle.YearsGroupView
            Me.VistaCalendarViewStyle = DirectCast(DevExpress.XtraEditors.VistaCalendarViewStyle.YearsGroupView Or DevExpress.XtraEditors.VistaCalendarInitialViewStyle.CenturyView, DevExpress.XtraEditors.VistaCalendarViewStyle)
            'Me.DisplayFormat.FormatString = "yyyy"
            'Me.EditFormat.FormatType = FormatType.Numeric
            'Me.EditFormat.FormatString = "N00"
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
                m.UseMaskAsDisplayFormat = True
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

        <Browsable(False)>
        Public Overrides Property ShowToday As Boolean
            Get
                Return False
            End Get
            Set(value As Boolean)
                MyBase.ShowToday = False
            End Set
        End Property

        <Browsable(False)>
        Public Overrides Property ShowClear As Boolean
            Get
                Return False
            End Get
            Set(value As Boolean)
                MyBase.ShowClear = False
            End Set
        End Property

        <Browsable(False)>
        Public Overrides Property VistaDisplayMode As DefaultBoolean
            Get
                Return DefaultBoolean.True
            End Get
            Set(value As DefaultBoolean)
                MyBase.VistaDisplayMode = DefaultBoolean.True
            End Set
        End Property

        Public ReadOnly Property SpinButton As EditorButton
        'Public ReadOnly Property SpinButtonDown As EditorButton
        Protected Overrides Function CreateButtonCollection() As EditorButtonCollection
            Dim col As EditorButtonCollection = MyBase.CreateButtonCollection()
            _SpinButton = New EditorButton(ButtonPredefines.SpinUp) With {.Tag = "SPIN"}
            '_SpinButtonDown = New EditorButton(ButtonPredefines.SpinUp) With {.Tag = "SPIN"}
            col.Add(_SpinButton)
            'col.Add(_SpinButtonDown)
            'Me.SpinButtonIndex = 1
            Return col
            'Return MyBase.CreateButtonCollection()
        End Function

        'Protected Overrides Sub RaiseButtonClick(e As ButtonPressedEventArgs)
        '    MyBase.RaiseButtonClick(e)
        'End Sub

        Protected Overrides Sub RaiseButtonPressed(e As ButtonPressedEventArgs)
            If e.Button.Kind = ButtonPredefines.SpinUp OrElse e.Button.Kind = ButtonPredefines.SpinDown Then
                Dim year As Integer = Me.YearEdit.ConvertToYear(Me.YearEdit.EditValue)
                If e.Button.Kind = ButtonPredefines.SpinUp Then
                    year += 1
                Else
                    year -= 1
                End If
                Me.YearEdit.EditValue = year
            End If
            MyBase.RaiseButtonPressed(e)

        End Sub


        Public Overrides Function GetDisplayText(editValue As Object) As String
            If editValue Is Nothing Then Return ""
            If TypeOf editValue Is Date Then
                Return DirectCast(editValue, Date).Year.ToString
            ElseIf TypeOf editValue Is Integer Then
                Return DirectCast(editValue, Integer).ToString()
            End If
            Return String.Empty
        End Function


        Public ReadOnly Property YearEdit As YearEdit
            Get
                Return DirectCast(Me.OwnerEdit, YearEdit)
            End Get
        End Property

        'Private _Year As Integer
        'Public Property Year As Integer
        '    Get
        '        Return _Year
        '    End Get
        '    Set(value As Integer)
        '        If _Year = value Then Return
        '        _Year = value
        '    End Set
        'End Property


        Protected Overrides Sub RaiseEditValueChangedCore(e As EventArgs)
            MyBase.RaiseEditValueChangedCore(e)
            Me.YearEdit.SetOldValue(Me.YearEdit)

        End Sub

        'Register the editor
        Public Shared Sub RegisterEdit()
            'Icon representing the editor within a container editor's Designer
            Dim img As Image = Nothing
            Try
                img = CType(Bitmap.FromStream(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("DevExpress.CustomEditors.CustomEdit.bmp")), Bitmap)
            Catch
            End Try
            'EditorRegistrationInfo.Default.Editors.Add(New EditorClassInfo(CustomEditName, GetType(YearEdit), GetType(RepositoryItemYearEdit), GetType(DateEditViewInfo), New ButtonEditPainter(), True, img))
            EditorRegistrationInfo.Default.Editors.Add(New EditorClassInfo(CustomEditName, GetType(YearEdit), GetType(RepositoryItemYearEdit), GetType(SpinButtonEditExViewInfo), New ButtonEditPainter(), True, img))
        End Sub

        'Override the Assign method
        Public Overrides Sub Assign(ByVal item As RepositoryItem)
            BeginUpdate()
            Try
                MyBase.Assign(item)
                Dim source As RepositoryItemYearEdit = TryCast(item, RepositoryItemYearEdit)
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

        'Overrides popup


        Protected Overrides Sub RaiseQueryPopUp(e As CancelEventArgs)
            MyBase.RaiseQueryPopUp(e)

            'Dim edit As DateCriteriaEdit = DirectCast(Me.OwnerEdit, DateCriteriaEdit)
            'If Me.EditMode = EditModeEnum.Between AndAlso edit.CriteriaPopUp IsNot Nothing Then
            '    edit.CriteriaPopUp.Criteria = edit.Criteria
            'End If
        End Sub
    End Class
End Namespace

