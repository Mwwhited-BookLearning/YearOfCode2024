package providers

import (
	"hybrid-search/webapi/models"
)

type BlobProvider interface {
	GetContent(file string) models.ContentReference
	List() []models.SearchResultModel
	TryStore(full string, file string, pathHash string) bool
}
