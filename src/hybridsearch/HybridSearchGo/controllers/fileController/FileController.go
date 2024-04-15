package fileController

import (
	"github.com/gin-gonic/gin"

	"hybrid-search/webapi/controllers"
	"hybrid-search/webapi/providers"

	"log"
	"strconv"
)

type FileController struct {
	Actions []controllers.WebAction
	Router  *gin.Engine

	HybridSearchProvider   providers.HybridSearchProvider
	LexicalSearchProvider  providers.LexicalSearchProvider
	SemanticSearchProvider providers.SemanticSearchProvider

	EmbeddingProvider providers.EmbeddingProvider

	DocumentBlobProvider providers.BlobProvider
	SummaryBlobProvider  providers.BlobProvider
}

func Build(
	router *gin.Engine,
	hybrid providers.HybridSearchProvider,
	lexical providers.LexicalSearchProvider,
	semantic providers.SemanticSearchProvider,
	embedding providers.EmbeddingProvider,
	documentBlob providers.BlobProvider,
	summaryBlob providers.BlobProvider) FileController {

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
		{Pattern: "/file/download/*path", Handler: service.Download, Method: "GET"},
		{Pattern: "/file/text/*path", Handler: service.Text, Method: "GET"},
		{Pattern: "/file/html/*path", Handler: service.Html, Method: "GET"},
		{Pattern: "/file/pdf/*path", Handler: service.Pdf, Method: "GET"},
		{Pattern: "/file/summary/*path", Handler: service.Summary, Method: "GET"},

		{Pattern: "/file/list", Handler: service.List, Method: "GET"},

		{Pattern: "/file/embed", Handler: service.Embed, Method: "GET"},

		{Pattern: "/file/semantic", Handler: service.Semantic, Method: "GET"},
		{Pattern: "/file/lexical", Handler: service.Lexical, Method: "GET"},
		{Pattern: "/file/hybrid", Handler: service.Hybrid, Method: "GET"},
	}
	service.Actions = actions

	for idx, ctrl := range service.Actions {
		log.Printf("FileController (%v): %s - %s", idx, ctrl.Pattern, ctrl.Method)

		if ctrl.Method == "GET" {
			router.GET(ctrl.Pattern, ctrl.Handler)
		} else if ctrl.Method == "POST" {
			router.POST(ctrl.Pattern, ctrl.Handler)
		}
	}

	return service
}

func (ctrl FileController) Download(context *gin.Context) {
	path := context.Param("path")
	log.Printf("Download: %s", path)
	//TODO: finish him!
	content := ctrl.DocumentBlobProvider.GetContent(path)

	context.Header("ContentType", content.ContentType)
	context.Writer.Write(content.Content)
}

func (ctrl FileController) Text(context *gin.Context) {
	path := context.Param("path")
	log.Printf("Text: %s", path)
}

func (ctrl FileController) Html(context *gin.Context) {
	path := context.Param("path")
	log.Printf("Html: %s", path)
}

func (ctrl FileController) Pdf(context *gin.Context) {
	path := context.Param("path")
	log.Printf("Pdf: %s", path)
}

func (ctrl FileController) Summary(context *gin.Context) {
	path := context.Param("path")
	log.Printf("Summary: %s", path)
}

func (ctrl FileController) List(context *gin.Context) {
	log.Printf("List")

	result := ctrl.DocumentBlobProvider.List()
	context.JSON(200, result)
}

func (ctrl FileController) Embed(context *gin.Context) {
	log.Printf("Embed")

	text := context.Query("text")

	result, _ := ctrl.EmbeddingProvider.Embed(text)
	context.JSON(200, result)
}

func (ctrl FileController) Semantic(context *gin.Context) {
	log.Printf("Semantic")

	query := context.Query("query")
	limit, _ := strconv.Atoi(context.Query("limit"))

	context.Header("X-APP-query", query)
	context.Header("X-APP-limit", strconv.Itoa(limit))

	result := ctrl.SemanticSearchProvider.Search(query, limit)
	context.JSON(200, result)
}

func (ctrl FileController) Lexical(context *gin.Context) {
	log.Printf("Lexical")

	query := context.Query("query")
	limit, _ := strconv.Atoi(context.Query("limit"))

	context.Header("X-APP-query", query)
	context.Header("X-APP-limit", strconv.Itoa(limit))

	result := ctrl.LexicalSearchProvider.Search(query, limit)
	context.JSON(200, result)
}

func (ctrl FileController) Hybrid(context *gin.Context) {
	log.Printf("Hybrid")

	// writer http.ResponseWriter, request *http.Request,
	query := context.Query("query")
	limit, _ := strconv.Atoi(context.Query("limit"))

	context.Header("X-APP-query", query)
	context.Header("X-APP-limit", strconv.Itoa(limit))

	result := ctrl.HybridSearchProvider.Search(query, limit)

	context.JSON(200, result)
}
