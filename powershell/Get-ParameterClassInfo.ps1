function Get-ParameterClassInfo
{
    [CmdletBinding()]
    Param (
        [Parameter(ValueFromPipeline = $true)]
        $ParameterClass
    )

    begin
    {
        $attribute = Find-PSMDType -FullName ParameterClasses.ParameterContractAttribute
        $contractType = Find-PSMDType -FullName ParameterClasses.ParameterContractType
        $contractBehavior = Find-PSMDType -FullName ParameterClasses.ParameterContractBehavior
    }

    process
    {
        foreach ($class in $ParameterClass)
        {
            $fields = $class.GetFields() | Where-Object { $_.CustomAttributes.AttributeType -contains $attribute }
            foreach ($field in $fields)
            {
                $field.CustomAttributes | Where-Object { $_.AttributeType -eq $attribute } | Select-Object -First 1 | ForEach-Object {
                    [PSCustomObject]@{
                        PSTypeName = "ParameterClasses.InfoItem"
                        Name = $field.Name
                        Type = [ParameterClasses.ParameterContractType](($field.CustomAttributes.ConstructorArguments | Where-Object ArgumentType -eq $contractType).Value)
                        Behavior = [ParameterClasses.ParameterContractBehavior](($field.CustomAttributes.ConstructorArguments | Where-Object ArgumentType -eq $contractBehavior).Value)
                        Class = $class
                    }
                }
            }

            $methods = $class.GetMethods() | Where-Object { $_.CustomAttributes.AttributeType -contains $attribute }
            foreach ($method in $methods)
            {
                $method.CustomAttributes | Where-Object { $_.AttributeType -eq $attribute } | Select-Object -First 1 | ForEach-Object {
                    $name = $method.Name
                    if ($name -eq "op_implicit")
                    {
                        $name = "I:{0}-->{1}" -f $method.GetParameters().ParameterType.Name, $method.ReturnType.Name
                    }
                    if ($name -eq "op_explicit")
                    {
                        $name = "E:{0}-->{1}" -f $method.GetParameters().ParameterType.Name, $method.ReturnType.Name
                    }
                    [PSCustomObject]@{
                        PSTypeName = "ParameterClasses.InfoItem"
                        Name = $name
                        Type = [ParameterClasses.ParameterContractType](($method.CustomAttributes.ConstructorArguments | Where-Object ArgumentType -eq $contractType).Value)
                        Behavior = [ParameterClasses.ParameterContractBehavior](($method.CustomAttributes.ConstructorArguments | Where-Object ArgumentType -eq $contractBehavior).Value)
                        Class = $class
                    }
                }
            }
        }
    }
}