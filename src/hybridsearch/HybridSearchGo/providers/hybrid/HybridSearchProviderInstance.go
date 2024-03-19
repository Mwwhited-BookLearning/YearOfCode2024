package hybrid

import (
	"hybrid-search/webapi/models"
	"hybrid-search/webapi/providers"
)

type HybridSearchProviderInstance struct {
	Lexical  providers.LexicalSearchProvider
	Semantic providers.SemanticSearchProvider
}

func Create(
	lexical providers.LexicalSearchProvider,
	semantic providers.SemanticSearchProvider) HybridSearchProviderInstance {
	return HybridSearchProviderInstance{
		Lexical:  lexical,
		Semantic: semantic,
	}
}

func (provider HybridSearchProviderInstance) Search(query string, limit int) []models.SearchResultWithSummaryModel {
	return nil //TODO: complete this
}
