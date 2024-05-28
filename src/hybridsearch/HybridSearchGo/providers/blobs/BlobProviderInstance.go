package blobs

import (
	"context"
	"github.com/Azure/azure-sdk-for-go/sdk/storage/azblob"
	"hybrid-search/webapi/models"
	"log"
)

type BlobProviderInstance struct {
	Client         *azblob.Client
	CollectionName string
}

func (provider BlobProviderInstance) GetContent(file string) models.ContentReference {
	//exists, _ := provider.Client.ServiceClient().GetProperties(context.Background(), nil)

	//TODO: do this better
	buffer := make([]byte, 1024*1024*1024)

	ret, err := provider.Client.DownloadBuffer(context.TODO(), provider.CollectionName, file, buffer, nil)

	log.Printf("GetContent-ret %v", ret, err)

	content := models.ContentReference{
		ContentType: "x-text/markdown", //TODO: fix this
		FileName:    file,
		Content:     buffer[:ret],
	}

	return content
}
func (provider BlobProviderInstance) List() []models.SearchResultModel {

	var result []models.SearchResultModel

	pager := provider.Client.NewListBlobsFlatPager(provider.CollectionName, nil)
	for pager.More() {
		resp, _ := pager.NextPage(context.TODO()) //TODO: do something with the error
		for _, item := range resp.Segment.BlobItems {
			model := models.SearchResultModel{
				Content:  "", //TODO: do something else here
				File:     *item.Metadata["file"],
				PathHash: *item.Metadata["pathhash"],
				Score:    1,
				Type:     models.SearchTypeNone,
			}
			result = append(result, model)
		}
	}

	return result
}
func (provider BlobProviderInstance) TryStore(full string, file string, pathHash string) bool {
	return false
}
