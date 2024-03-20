package blobs

import (
	"github.com/Azure/azure-sdk-for-go/sdk/storage/azblob"
	"hybrid-search/webapi/models"
)

type BlobProviderInstance struct {
	Client         *azblob.Client
	CollectionName string
}

func (provider BlobProviderInstance) GetContent(file string) models.ContentReference {
	return models.ContentReference{}
}
func (provider BlobProviderInstance) List() []models.SearchResultModel {
	return []models.SearchResultModel{}
}
func (provider BlobProviderInstance) TryStore(full string, file string, pathHash string) bool {
	return false
}
