﻿Imports System.ComponentModel
Imports AcurSoft.XtraEditors.Base
Imports AcurSoft.XtraEditors.ViewInfo
Imports DevExpress.Utils
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraEditors.Drawing
Imports DevExpress.XtraEditors.Mask
Imports DevExpress.XtraEditors.Registrator
Imports DevExpress.XtraEditors.Repository


Namespace AcurSoft.XtraEditors.Repository
    <UserRepositoryItem("RegisterMonthEditEx")>
    Public Class RepositoryItemMonthEditEx
        Inherits RepositoryItemLookUpEdit
        Implements ISpinButtonEditor
        'The unique name for the custom editor
        Public Const CustomEditName As String = "MonthEditEx"


        Private Shared _Months As Dictionary(Of Integer, String)
        Public Shared ReadOnly Property Months As Dictionary(Of Integer, String)
            Get
                If _Months Is Nothing Then
                    _Months = New Dictionary(Of Integer, String)
                    Dim dateFoo As New Date(2016, 1, 1)
                    For i As Integer = 1 To 12
                        _Months.Add(i, String.Format("{0:d2}: {1}", i, Globalization.CultureInfo.InvariantCulture.TextInfo.ToTitleCase(dateFoo.AddMonths(i - 1).ToString("MMMM"))))
                    Next
                End If
                Return _Months
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
            Me.DataSource = RepositoryItemMonthEditEx.Months
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

        Protected Overrides Sub RaiseEditValueChanging(e As ChangingEventArgs)
            MyBase.RaiseEditValueChanging(e)
            If e.NewValue Is Nothing Then
                e.Cancel = True
            Else
                Try
                    Dim month As Integer = Convert.ToInt32(e.NewValue)
                    If month < 1 OrElse month > 12 Then
                        e.Cancel = True
                    End If
                Catch ex As Exception
                    e.Cancel = True
                End Try
            End If
        End Sub


        Protected Overrides Sub RaiseButtonPressed(e As ButtonPressedEventArgs)
            If e.Button.Kind = ButtonPredefines.SpinUp OrElse e.Button.Kind = ButtonPredefines.SpinDown Then
                Dim month As Integer = Convert.ToInt32(Me.MonthEditEx.EditValue)
                If e.Button.Kind = ButtonPredefines.SpinUp Then
                    month += 1
                Else
                    month -= 1
                End If
                If month = 13 Then
                    month = 1
                ElseIf month = 0
                    month = 12
                End If
                Me.MonthEditEx.EditValue = month
            End If
            MyBase.RaiseButtonPressed(e)

        End Sub
        Public ReadOnly Property MonthEditEx As MonthEditEx
            Get
                Return DirectCast(Me.OwnerEdit, MonthEditEx)
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
            EditorRegistrationInfo.Default.Editors.Add(New EditorClassInfo(CustomEditName, GetType(MonthEditEx), GetType(RepositoryItemMonthEditEx), GetType(SpinButtonEditExViewInfo), New ButtonEditPainter, True, img))
        End Sub

        'Override the Assign method
        Public Overrides Sub Assign(ByVal item As RepositoryItem)
            BeginUpdate()
            Try
                MyBase.Assign(item)
                Dim source As RepositoryItemMonthEditEx = TryCast(item, RepositoryItemMonthEditEx)
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

