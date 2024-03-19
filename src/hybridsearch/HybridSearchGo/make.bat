
@ECHO OFF
SET cmd=%1

CALL :%cmd%
EXIT /B 0

:tools
go install ^
github.com/deepmap/oapi-codegen/cmd/oapi-codegen ^
github.com/fdaines/spm-go ^
github.com/golangci/golangci-lint/cmd/golangci-lint ^
github.com/jackc/tern/v2 ^
github.com/maxbrunsfeld/counterfeiter/v6 ^
github.com/sqlc-dev/sqlc/cmd/sqlc ^
goa.design/model/cmd/mdl ^
goa.design/model/cmd/stz
EXIT /B 0

:generate
go generate .
EXIT /B 0

:golangci
golangci-lint run .
EXIT /B 0

:setup
go get github.com/ghodss/yaml
EXIT /B 0

:run
go run .
EXIT /B 0
