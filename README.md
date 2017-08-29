# yanwei2508.github.io

### This is my blog about software development

- First, we'll look into Elasticsearch
- Then we'll set geohashing
- Finally, we'll **celebrate!**

---

Here's a [link](https://www.linkedin.com/in/%E7%87%95%E4%BC%9F-%E8%AE%B8-ba37b2b1/) to my LinkedIn profile

```
public static ISearchResponse<Bank> esSearchNumber()
        {
            //
            string dictionaryKey = "balance";
            var dictionary = Extension.BankDictionary();
            var rangeField = dictionary[dictionaryKey];
            var gt = 40000;
            var lt = 40100;
            var searchResponse = elastic.Search<Bank>(es => es
              .Query(q => q
                    .Range(r => r
                        .Name("")
                        .Field(rangeField)
                            .GreaterThan(gt)
                            .LessThan(lt)
                            .Boost(2.0))));
            return searchResponse;
        }
```
