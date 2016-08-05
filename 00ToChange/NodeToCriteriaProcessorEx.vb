Imports AcurSoft.XtraEditors.Filtering
Imports DevExpress.Data
Imports DevExpress.Data.Filtering
Imports DevExpress.Data.Filtering.Helpers

Namespace AcurSoft.Data.Filtering.Helpers
    Public Class NodeToCriteriaProcessorEx
        Inherits NodeToCriteriaProcessor

        Public ReadOnly Property Model As WinFilterTreeNodeModelEx

        Public Sub New(model As WinFilterTreeNodeModelEx)
            _Model = model
        End Sub

        Public Overrides Function Visit(ByVal icn As IClauseNode) As Object
            If icn.Operation.IsDefined() Then
                'TODO: Replace incompatible OperandProperty (having a type <> from the argument type)

                Return MyBase.Visit(icn)
            End If
            With Utils.XNullable(Of Object).Instance(CreateCustomOperation(icn, icn.Operation.IntegerValue))
                If .HasValue Then
                    Return .Value
                End If
            End With
            Return MyBase.Visit(icn)
        End Function

        Private Function CreateCustomOperation(ByVal icn As IClauseNode, ByVal operation As Integer) As Object
            Dim info As XpoFunctionFilterInfo = XpoFunctionFilterManager.GetInfos(operation)
            If info IsNot Nothing Then
                'info.Parameters(0).ParameterInfo.ParameterType
                'Dim firstOperandName As String = icn.FirstOperand.PropertyName.Trim.ToUpper
                'Dim firstOperandType As Type = Me.Model.FilterProperties.OfType(Of IBoundProperty).FirstOrDefault(Function(q) q.Name.ToUpper() = firstOperandName)?.Type
                Dim firstOperandType As Type = DirectCast(icn, ClauseNodeEx).GetFirstOperandType()
                If info.Parameters(0).ParameterInfo.ParameterType Is firstOperandType Then

                    'End If

                    'If TypeOf info.XpoFunction Is XpoFunctionForDateRanges Then
                    'Dim xpoFunctionForDateRanges As XpoFunctionForDateRanges = DirectCast(info.XpoFunction, XpoFunctionForDateRanges)
                    Dim crs As New List(Of CriteriaOperator)

                    crs.Add(New ConstantValue(info.FunctionName))
                    crs.Add(icn.FirstOperand)
                    Dim additionalOperands As New List(Of CriteriaOperator)
                    'Dim defaultOperands As List(Of CriteriaOperator) = xpoFunctionForDateRanges.DefaultParametersCriteria.Skip(1).ToList()

                    Dim defaultOperands As New List(Of CriteriaOperator)
                    For i As Integer = 0 To info.Parameters.Count - 2
                        defaultOperands.Add(New OperandParameter())
                    Next
                    '= xpoFunctionForDateRanges.DefaultParametersCriteria.Skip(1).ToList()
                    For i As Integer = 0 To defaultOperands.Count - 1
                        If i >= icn.AdditionalOperands.Count OrElse (TypeOf icn.AdditionalOperands(i) Is OperandValue AndAlso DirectCast(icn.AdditionalOperands(i), OperandValue).Value Is Nothing) Then
                            additionalOperands.Add(defaultOperands(i))
                        Else
                            additionalOperands.Add(icn.AdditionalOperands(i))
                        End If
                    Next

                    crs.AddRange(additionalOperands)
                    Dim cnt As Integer = icn.AdditionalOperands.Count

                    If icn.AdditionalOperands.Count < additionalOperands.Count Then
                        For i As Integer = cnt To additionalOperands.Count - 1
                            icn.AdditionalOperands.Add(New OperandValue)
                        Next
                    ElseIf icn.AdditionalOperands.Count > additionalOperands.Count
                        While icn.AdditionalOperands.Count > additionalOperands.Count
                            icn.AdditionalOperands.RemoveAt(icn.AdditionalOperands.Count - 1)
                        End While
                    End If

                    Dim toReplace As New Dictionary(Of Integer, Object)

                    For i As Integer = 0 To icn.AdditionalOperands.Count - 1
                        Dim paramIndex As Integer = i + 1
                        Dim cr As CriteriaOperator = icn.AdditionalOperands(i)
                        If TypeOf cr Is OperandValue AndAlso TypeOf defaultOperands(i) Is OperandValue Then
                            DirectCast(icn.AdditionalOperands(i), OperandValue).Value = info.Parameters(paramIndex).ValueFixer.Invoke(DirectCast(icn.AdditionalOperands(i), OperandValue).Value) ' DirectCast(defaultOperands(i), OperandValue).Value
                        ElseIf TypeOf cr Is OperandProperty AndAlso TypeOf defaultOperands(i) Is OperandValue Then
                            Dim op As OperandProperty = DirectCast(cr, OperandProperty)
                            Dim opType As Type = Me.Model.FilterProperties.Item(op.PropertyName)?.Type
                            If opType IsNot Nothing Then
                                Dim propType As Type = info.Parameters(paramIndex).ParameterInfo.ParameterType
                                If propType IsNot opType Then
                                    Dim bp As IBoundProperty = Me.Model.FilterProperties.OfType(Of IBoundProperty).FirstOrDefault(Function(q) q.Type.IsCastableTo(propType))
                                    If bp Is Nothing Then
                                        'toReplace.Add(i, info.Parameters(paramIndex).ValueFixer.Invoke(DirectCast(defaultOperands(i), OperandValue).Value))
                                        toReplace.Add(i, info.Parameters(paramIndex).ValueFixer.Invoke(Nothing))
                                    Else
                                        toReplace.Add(i, New OperandProperty(bp.Name))
                                    End If
                                End If
                            End If
                        End If

                        'Replace incompatible OperandProperty (having a type <> from the argument type)
                        For Each kv In toReplace
                            icn.AdditionalOperands.RemoveAt(kv.Key)
                            If TypeOf kv.Value Is OperandProperty Then
                                icn.AdditionalOperands.Insert(kv.Key, DirectCast(kv.Value, OperandProperty))
                            Else
                                icn.AdditionalOperands.Insert(kv.Key, New OperandValue(kv.Value))
                            End If
                        Next
                    Next

                    Return New FunctionOperator(FunctionOperatorType.Custom, crs.ToArray)
                End If
            End If

            If operation = ClauseTypeEnumHelper.MatchesAnyOf Then
                Return New FunctionOperator(FunctionOperatorType.Custom, New CriteriaOperator() {
                    New ConstantValue("MatchesAnyOf"),
                    icn.FirstOperand
                }.Union(icn.AdditionalOperands))
            End If
            Return Nothing
            'Throw New NotImplementedException()
        End Function
    End Class

End Namespace
