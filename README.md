# SensitiveWordFilter
敏感词过滤系统
xlsx导表成lua文件(自己写) -> lua文件 -> filterHandler处理构建字典树->正则交由RegexHelper处理,普通串交由filterHandler的checkFilter处理,返回是否通过,替换后的词串,不通过原因
