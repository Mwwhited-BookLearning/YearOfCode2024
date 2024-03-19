package fileController

import (
	//"encoding/json"
	"log"
	"net/http"
	"strings"

	"github.com/gorilla/mux"
)

type RouteTypes int

const (
	PathFilter RouteTypes = 0
	Query                 = 1
)

type FileAction struct {
	Pattern   string
	Handler   func(http.ResponseWriter, *http.Request)
	Method    string
	RouteType RouteTypes
}

type FileService interface {
	CreateRoutes() []FileAction
	AddRoutes(routes []FileAction)
}

func ListRoutes() []FileAction {
	return []FileAction{
		{Pattern: "/file/download", Handler: Download, Method: "GET", RouteType: PathFilter},
		{Pattern: "/file/text", Handler: Text, Method: "GET", RouteType: PathFilter},
		{Pattern: "/file/html", Handler: Html, Method: "GET", RouteType: PathFilter},
		{Pattern: "/file/pdf", Handler: Pdf, Method: "GET", RouteType: PathFilter},
		{Pattern: "/file/summary", Handler: Summary, Method: "GET", RouteType: PathFilter},
		{Pattern: "/file/embed", Handler: Embed, Method: "GET", RouteType: Query},
		{Pattern: "/file/semantic", Handler: Semantic, Method: "GET", RouteType: Query},
		{Pattern: "/file/lexical", Handler: Lexical, Method: "GET", RouteType: Query},
		{Pattern: "/file/hybrid", Handler: Hybrid, Method: "GET", RouteType: Query},
	}
}

func BindRoutes(router *mux.Router, routes []FileAction) {
	// https://github.com/gorilla/mux
	for _, ctrl := range routes {
		log.Printf("FileController: %s - %s", ctrl.Pattern, ctrl.Method)
		if ctrl.RouteType == PathFilter {
			router.PathPrefix(ctrl.Pattern).HandlerFunc(ctrl.Handler)
		} else if ctrl.RouteType == Query {
			router.HandleFunc(ctrl.Pattern, ctrl.Handler)
		}
	}
}

func AddRoutes(router *mux.Router) {
	routes := ListRoutes()
	BindRoutes(router, routes)
}

func getPath(request *http.Request, basePath string) string {
	path := request.URL.Path[len(basePath):]
	if strings.HasPrefix(path, "/") {
		path = path[1:]
	}
	return path
}

func Download(writer http.ResponseWriter, request *http.Request) {
	path := getPath(request, "/file/download/")
	log.Printf("Download: %s", path)
}

func Text(writer http.ResponseWriter, request *http.Request) {
	path := getPath(request, "/file/text/")
	log.Printf("Text: %s", path)
}

func Html(writer http.ResponseWriter, request *http.Request) {
	path := getPath(request, "/file/html/")
	log.Printf("Html: %s", path)
}

func Pdf(writer http.ResponseWriter, request *http.Request) {
	path := getPath(request, "/file/pdf/")
	log.Printf("Pdf: %s", path)
}

func Summary(writer http.ResponseWriter, request *http.Request) {
	path := getPath(request, "/file/summary/")
	log.Printf("Summary: %s", path)
}

func Embed(writer http.ResponseWriter, request *http.Request) {
	log.Printf("Embed")
}

func Semantic(writer http.ResponseWriter, request *http.Request) {
	log.Printf("Semantic")
}

func Lexical(writer http.ResponseWriter, request *http.Request) {
	log.Printf("Lexical")
}

func Hybrid(writer http.ResponseWriter, request *http.Request) {
	log.Printf("Hybrid")
}
