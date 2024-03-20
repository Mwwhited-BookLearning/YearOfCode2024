package fileController

import (
	"encoding/json"

	"github.com/gorilla/mux"

	"hybrid-search/webapi/controllers"
	"hybrid-search/webapi/providers"

	"log"
	"net/http"
	"strconv"
	"strings"
)

type FileController struct {
	Actions []controllers.WebAction
	Router  *mux.Router

	HybridSearchProvider   providers.HybridSearchProvider
	LexicalSearchProvider  providers.LexicalSearchProvider
	SemanticSearchProvider providers.SemanticSearchProvider

	EmbeddingProvider providers.EmbeddingProvider

	DocumentBlobProvider providers.BlobProvider
	SummaryBlobProvider  providers.BlobProvider
}

func Build(
	router *mux.Router,
	hybrid providers.HybridSearchProvider,
	lexical providers.LexicalSearchProvider,
	semantic providers.SemanticSearchProvider,
	embedding providers.EmbeddingProvider,
	documentBlob providers.BlobProvider,
	summaryBlob providers.BlobProvider) FileController {
	// https://github.com/gorilla/mux

	service := FileController{
		Router: router,

		HybridSearchProvider:   hybrid,
		LexicalSearchProvider:  lexical,
		SemanticSearchProvider: semantic,

		EmbeddingProvider: embedding,

		DocumentBlobProvider: documentBlob,
		SummaryBlobProvider:  summaryBlob,
	}

	actions := []controllers.WebAction{
		{Pattern: "/file/download", Handler: service.Download, Method: "GET", RouteType: controllers.RouteTypePathFilter},
		{Pattern: "/file/text", Handler: service.Text, Method: "GET", RouteType: controllers.RouteTypePathFilter},
		{Pattern: "/file/html", Handler: service.Html, Method: "GET", RouteType: controllers.RouteTypePathFilter},
		{Pattern: "/file/pdf", Handler: service.Pdf, Method: "GET", RouteType: controllers.RouteTypePathFilter},
		{Pattern: "/file/summary", Handler: service.Summary, Method: "GET", RouteType: controllers.RouteTypePathFilter},

		{Pattern: "/file/list", Handler: service.List, Method: "GET", RouteType: controllers.RouteTypeQuery},

		{Pattern: "/file/embed", Handler: service.Embed, Method: "GET", RouteType: controllers.RouteTypeQuery},

		{Pattern: "/file/semantic", Handler: service.Semantic, Method: "GET", RouteType: controllers.RouteTypeQuery},
		{Pattern: "/file/lexical", Handler: service.Lexical, Method: "GET", RouteType: controllers.RouteTypeQuery},
		{Pattern: "/file/hybrid", Handler: service.Hybrid, Method: "GET", RouteType: controllers.RouteTypeQuery},
	}
	service.Actions = actions

	for idx, ctrl := range service.Actions {
		log.Printf("FileController (%v): %s - %s", idx, ctrl.Pattern, ctrl.Method)
		if ctrl.RouteType == controllers.RouteTypePathFilter {
			router.PathPrefix(ctrl.Pattern).HandlerFunc(ctrl.Handler)
		} else if ctrl.RouteType == controllers.RouteTypeQuery {
			router.HandleFunc(ctrl.Pattern, ctrl.Handler)
		}
	}

	return service
}

func getPath(request *http.Request, basePath string) string {
	path := request.URL.Path[len(basePath):]
	if strings.HasPrefix(path, "/") {
		path = path[1:]
	}
	return path
}

func (ctrl FileController) Download(writer http.ResponseWriter, request *http.Request) {
	path := getPath(request, "/file/download/")
	log.Printf("Download: %s", path)
	//TODO: finish him!
	//content := ctrl.DocumentBlobProvider.GetContent(path)
}

func (ctrl FileController) Text(writer http.ResponseWriter, request *http.Request) {
	path := getPath(request, "/file/text/")
	log.Printf("Text: %s", path)
}

func (ctrl FileController) Html(writer http.ResponseWriter, request *http.Request) {
	path := getPath(request, "/file/html/")
	log.Printf("Html: %s", path)
}

func (ctrl FileController) Pdf(writer http.ResponseWriter, request *http.Request) {
	path := getPath(request, "/file/pdf/")
	log.Printf("Pdf: %s", path)
}

func (ctrl FileController) Summary(writer http.ResponseWriter, request *http.Request) {
	path := getPath(request, "/file/summary/")
	log.Printf("Summary: %s", path)
}

func (ctrl FileController) List(writer http.ResponseWriter, request *http.Request) {
	log.Printf("List")

	result := ctrl.DocumentBlobProvider.List()
	json.NewEncoder(writer).Encode(result)
}

func (ctrl FileController) Embed(writer http.ResponseWriter, request *http.Request) {
	log.Printf("Embed")

	text := mux.Vars(request)["text"]

	result, _ := ctrl.EmbeddingProvider.Embed(text)
	json.NewEncoder(writer).Encode(result)
}

func (ctrl FileController) Semantic(writer http.ResponseWriter, request *http.Request) {
	log.Printf("Semantic")

	query := mux.Vars(request)["query"]
	limit, _ := strconv.Atoi(mux.Vars(request)["limit"])

	writer.Header().Add("X-APP-query", query)
	writer.Header().Add("X-APP-limit", strconv.Itoa(limit))

	result := ctrl.SemanticSearchProvider.Search(query, limit)
	json.NewEncoder(writer).Encode(result)
}

func (ctrl FileController) Lexical(writer http.ResponseWriter, request *http.Request) {
	log.Printf("Lexical")

	query := mux.Vars(request)["query"]
	limit, _ := strconv.Atoi(mux.Vars(request)["limit"])

	writer.Header().Add("X-APP-query", query)
	writer.Header().Add("X-APP-limit", strconv.Itoa(limit))

	result := ctrl.LexicalSearchProvider.Search(query, limit)
	json.NewEncoder(writer).Encode(result)
}

func (ctrl FileController) Hybrid(writer http.ResponseWriter, request *http.Request) {
	log.Printf("Hybrid")

	query := mux.Vars(request)["query"]
	limit, _ := strconv.Atoi(mux.Vars(request)["limit"])

	writer.Header().Add("X-APP-query", query)
	writer.Header().Add("X-APP-limit", strconv.Itoa(limit))

	result := ctrl.HybridSearchProvider.Search(query, limit)
	json.NewEncoder(writer).Encode(result)
}
