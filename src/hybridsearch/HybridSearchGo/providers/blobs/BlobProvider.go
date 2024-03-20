package blobs

import (
	"hybrid-search/webapi/models"
)

type BlobProvider interface {
	GetContent(file string) ContentReference
	List() []models.SearchResultModel
	TryStore(full string, file string, pathHash string) bool
}
