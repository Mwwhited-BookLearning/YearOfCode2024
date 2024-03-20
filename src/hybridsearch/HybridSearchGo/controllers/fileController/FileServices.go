package fileController

import (
	"net/http"
)

type FileServices interface {
	Download(writer http.ResponseWriter, request *http.Request)
	// Text(writer http.ResponseWriter, request *http.Request)
	// Html(writer http.ResponseWriter, request *http.Request)
	// Pdf(writer http.ResponseWriter, request *http.Request)
	// Summary(writer http.ResponseWriter, request *http.Request)
	List(writer http.ResponseWriter, request *http.Request)
	Embed(writer http.ResponseWriter, request *http.Request)
	Semantic(writer http.ResponseWriter, request *http.Request)
	Lexical(writer http.ResponseWriter, request *http.Request)
	Hybrid(writer http.ResponseWriter, request *http.Request)
}
