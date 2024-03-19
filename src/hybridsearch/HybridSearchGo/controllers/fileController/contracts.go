package fileController

import (
	"github.com/gorilla/mux"
	"hybrid-search/webapi/controllers"
	"hybrid-search/webapi/providers"
	"net/http"
)

type FileController struct {
	Actions        []controllers.WebAction
	Router         *mux.Router
	HybridSearch   providers.HybridSearchProvider
	LexicalSearch  providers.LexicalSearchProvider
	SemanticSearch providers.SemanticSearchProvider
}

type FileServices interface {
	Download(writer http.ResponseWriter, request *http.Request)
	Text(writer http.ResponseWriter, request *http.Request)
	Html(writer http.ResponseWriter, request *http.Request)
	Pdf(writer http.ResponseWriter, request *http.Request)
	Summary(writer http.ResponseWriter, request *http.Request)
	Embed(writer http.ResponseWriter, request *http.Request)
	Semantic(writer http.ResponseWriter, request *http.Request)
	Lexical(writer http.ResponseWriter, request *http.Request)
	Hybrid(writer http.ResponseWriter, request *http.Request)
}
